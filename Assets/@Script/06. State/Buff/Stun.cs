using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : BaseBuff
{
    public Stun()
    {
        buffType = BUFF_TYPE.DeBuff;
        buff = BUFF.Stun;
        duration = 0f;
    }

    public override void SetDuration(float duration)
    {
        // °»½Å
        if (this.duration < duration)
        {
            this.duration = duration;
        }
    }

    public override void EnableBuff(BaseActor actor)
    {
        actor.Animator.Play(animationParameter);
    }
    public override bool UpdateBuff(BaseActor actor)
    {
        if (duration > 0f)
        {
            duration -= Time.deltaTime;
            return true;
        }
        else
        {
            duration = 0f;
            return false;
        }
    }
    public override void DisableBuff(BaseActor actor)
    {

    }
}
