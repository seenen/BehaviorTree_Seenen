using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Monster")]
public class MonsterBirth : Action
{
    public SharedVector3 birthPos;

    public AnimationState state = null;

    public float clipduring = 0;

    public override void OnAwake()
    {
        //birthPos = (SharedVector3)GlobalVariables.Instance.GetVariable("MonsterBirth_Pos");
        birthPos = (SharedVector3)Owner.GetVariable("MonsterBirth_Pos");

        gameObject.transform.position = birthPos.Value;

        state = gameObject.animation["f_out"];
    }

    public override void OnStart()
    {
        base.OnStart();

        if (state != null)
        {
            clipduring = state.clip.length;

            gameObject.animation.Play(state.clip.name);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (state == null ||
            clipduring == 0)
        {
            return TaskStatus.Success;
        }

        clipduring -= Time.deltaTime;

        if (clipduring < 0.1f)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
