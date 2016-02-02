using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 是否在可视范围内.
/// </summary>
[TaskCategory("Common")]
public class IsCanSee : Conditional
{
    //  正在搜寻的对象
    public SharedTransform targetObject;

    //  搜寻的可视角度
    public SharedFloat fieldOfViewAngle = 90;

    //  搜寻的视野范围
    public SharedFloat viewDistance = 1000;

    //  视野内的对象
    public SharedTransform objectInSight;

    public SharedVector3 offset;

    public LayerMask objectLayerMask;

    // Reset the public variables
    public override void OnReset()
    {
        fieldOfViewAngle = 90;
        viewDistance = 1000;
    }

    public override TaskStatus OnUpdate()
    {
        // If the target object is null then determine if there are any objects within sight based on the layer mask
        if (targetObject.Value == null)
        {
            objectInSight.Value = BDUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask);
        }
        else { // If the target is not null then determine if that object is within sight
            objectInSight.Value = BDUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value);
        }

        if (objectInSight.Value != null)
        {
            // Return success if an object was found
            return TaskStatus.Success;
        }
        // An object is not within sight so return failure
        return TaskStatus.Failure;
    }

}
