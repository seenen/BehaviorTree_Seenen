using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Monster")]
public class MonsterMovement : Action
{
    public SharedFloat speed;

    public SharedFloat arriveDistance = 0.1f;

    public SharedBool lookAtTarget = true;

    public SharedTransform targetTransform;

    public SharedVector3 targetPosition;

    public SharedFloat maxLookAtRotationDelta;

    public AnimationState state = null;

    public override void OnAwake()
    {
        state = gameObject.animation["f_run"];

        state.wrapMode = WrapMode.Loop;
    }

    public override void OnStart()
    {
        targetPosition = (SharedVector3)GlobalVariables.Instance.GetVariable("MonsterMovement_Target");

        if ((targetTransform == null || targetTransform.Value == null) && targetPosition == null)
        {
            Debug.LogError("Error: A MoveTowards target value is not set.");
            targetPosition = new SharedVector3(); // create a new SharedVector3 to prevent repeated errors
        }

        gameObject.animation.Play(state.clip.name, PlayMode.StopAll);

    }

    public override TaskStatus OnUpdate()
    {
        var position = Target();
        // Return a task status of success once we've reached the target
        if (Vector3.SqrMagnitude(transform.position - position) < arriveDistance.Value)
        {
            return TaskStatus.Success;
        }

        // We haven't reached the target yet so keep moving towards it
        transform.position = Vector3.MoveTowards(transform.position, position, speed.Value * Time.deltaTime);
        if (lookAtTarget.Value)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(position - transform.position), maxLookAtRotationDelta.Value);
        }

        return TaskStatus.Running;
    }

    // Return targetPosition if targetTransform is null
    private Vector3 Target()
    {
        if (targetTransform == null || targetTransform.Value == null)
        {
            return targetPosition.Value;
        }
        return targetTransform.Value.position;
    }

    // Reset the public variables
    public override void OnReset()
    {
        arriveDistance = 0.1f;
        lookAtTarget = true;
    }

}
