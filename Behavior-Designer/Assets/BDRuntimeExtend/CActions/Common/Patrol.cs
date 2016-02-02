using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 巡逻
/// </summary>
[TaskDescription("公共方法，用于巡逻用")]
[TaskCategory("Common")]
[TaskIcon("Assets/BDRuntimeExtend/Editor/Icons/{SkinColor}PatrolIcon.png")]
public class Patrol : Action
{
    [Tooltip("巡逻的速度")]
    public SharedFloat speed;
    [Tooltip("转身的速度")]
    public SharedFloat angularSpeed;
    [Tooltip("到达的距离")]
    public SharedFloat arriveDistance = 0.1f;
    [Tooltip("是否随机路点")]
    public SharedBool randomPatrol = false;
    [Tooltip("路点")]
    public SharedTransformList waypoints = null;

    private int waypointIndex;

    private NavMeshAgent navMeshAgent;

    public override void OnAwake()
    {
        // cache for quick lookup
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    public override void OnStart()
    {
        // initially move towards the closest waypoint
        float distance = Mathf.Infinity;
        float localDistance;
        for (int i = 0; i < waypoints.Value.Count; ++i)
        {
            if ((localDistance = Vector3.Magnitude(transform.position - waypoints.Value[i].position)) < distance)
            {
                distance = localDistance;
                waypointIndex = i;
            }
        }

        // set the speed, angular speed, and destination then enable the agent
        navMeshAgent.speed = speed.Value;
        navMeshAgent.angularSpeed = angularSpeed.Value;
        navMeshAgent.enabled = true;
        navMeshAgent.destination = Target();
    }

    // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
    public override TaskStatus OnUpdate()
    {
        if (!navMeshAgent.pathPending)
        {
            var thisPosition = transform.position;
            thisPosition.y = navMeshAgent.destination.y; // ignore y
            if (Vector3.SqrMagnitude(thisPosition - navMeshAgent.destination) < arriveDistance.Value)
            {
                if (randomPatrol.Value)
                {
                    waypointIndex = Random.Range(0, waypoints.Value.Count);
                }
                else {
                    waypointIndex = (waypointIndex + 1) % waypoints.Value.Count;
                }
                navMeshAgent.destination = Target();
            }
        }

        return TaskStatus.Running;
    }

    // Return the current waypoint index position
    private Vector3 Target()
    {
        return waypoints.Value[waypointIndex].position;
    }

    // Reset the public variables
    public override void OnReset()
    {
        arriveDistance = 0.1f;
        waypoints = null;
        randomPatrol = false;
    }

    // Draw a gizmo indicating a patrol 
    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (waypoints == null ||
            waypoints.Value.Count == 0)
        {
            return;
        }
        var oldColor = UnityEditor.Handles.color;
        UnityEditor.Handles.color = Color.yellow;
        for (int i = 0; i < waypoints.Value.Count; ++i)
        {
            UnityEditor.Handles.SphereCap(0, waypoints.Value[i].position, waypoints.Value[i].rotation, 1);
        }
        UnityEditor.Handles.color = oldColor;
#endif
    }

}
