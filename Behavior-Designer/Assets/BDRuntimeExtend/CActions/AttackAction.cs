using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AttackAction : Action
{
    public AnimationClip clip = null;

    public float clipduring = 0;

    public override void OnAwake()
    {
        if (clip != null)
        {
            clipduring = clip.length;

            gameObject.animation.Play(clip.name);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (clip == null ||
            clipduring == 0)
        {
            return TaskStatus.Failure;
        }

        clipduring -= Time.deltaTime;

        if (clipduring < 0.1f)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
