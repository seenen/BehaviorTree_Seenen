using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Monster")]
public class MonsterDeath : Action
{
    public AnimationState state = null;

    public float clipduring = 0;

    public override void OnAwake()
    {
        //birthPos = (SharedVector3)GlobalVariables.Instance.GetVariable("MonsterBirth_Pos");
        state = gameObject.animation["f_dead"];
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
