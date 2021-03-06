﻿using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Monster")]
public class MonsterAttack : Action
{
    public AnimationState state = null;

    public float clipduring = 0;

    public override void OnAwake()
    {
        state = gameObject.animation["f_attack01"];

        state.wrapMode = WrapMode.Once;
    }

    public override void OnStart()
    {
        base.OnStart();

        if (state != null)
        {
            clipduring = state.clip.length;

            gameObject.animation.Play(state.clip.name, PlayMode.StopAll);
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
