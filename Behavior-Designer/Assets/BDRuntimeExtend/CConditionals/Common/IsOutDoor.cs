using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 是否在户外.
/// </summary>
[TaskCategory("Common")]
public class IsOutDoor : Conditional
{
    //  是否在户外.
    SharedBool isOutDoor = false;

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {
        if (isOutDoor.Value)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
