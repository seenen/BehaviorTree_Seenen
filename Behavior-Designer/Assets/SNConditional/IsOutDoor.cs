using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 是否在户外.
/// </summary>
public class IsOutDoor : Conditional
{
    //  是否在户外.
    public bool isOutDoor = false;

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {
        if (isOutDoor)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
