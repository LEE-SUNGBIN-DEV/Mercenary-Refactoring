using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebuffBurn : BaseDebuff<BaseCharacter>
{
    private float interval;
    private float ratio;

    public PlayerDebuffBurn(DebuffData debuffData) : base(debuffData)
    {
        timer = 0f;
        interval = 0f;
        ratio = 0f;
    }

    public override void SetLifetime(float lifetime)
    {
        // °»½Å
        if (this.lifetime < lifetime)
        {
            this.lifetime = lifetime;
        }
    }

    public override void Enable(BaseCharacter actor)
    {
        timer = 0f;
    }

    public override void Update(BaseCharacter actor)
    {
        timer += Time.deltaTime;
        if (lifetime > 0f)
        {
            lifetime -= Time.deltaTime;
            if(timer >= interval)
            {
                timer -= interval;
                actor.Status.CurrentHP -= (actor.Status.MaxHP * ratio);
            }
        }
        else
        {
            lifetime = 0f;
        }
    }
    public override void Disable(BaseCharacter actor)
    {
        timer = 0f;
    }
}
