using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

/// <summary>
/// 是否在户外.
/// </summary>
[TaskCategory("Common")]
public class IsAttacked : Conditional
{
    //  是否被攻击.
    SharedBool isAttacked = false;

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {
        isAttacked = (SharedBool)GlobalVariables.Instance.GetVariable("IsAttacked");

        if (isAttacked.Value)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

}
