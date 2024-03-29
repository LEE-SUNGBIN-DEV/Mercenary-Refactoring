using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebuffBurn : BaseDebuff<PlayerCharacter>
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
        // ����
        if (this.lifetime < lifetime)
        {
            this.lifetime = lifetime;
        }
    }

    public override void Enable(PlayerCharacter actor)
    {
        timer = 0f;
    }

    public override void Update(PlayerCharacter actor)
    {
        timer += Time.deltaTime;
        if (lifetime > 0f)
        {
            lifetime -= Time.deltaTime;
            if(timer >= interval)
            {
                timer -= interval;
                actor.StatusData.ReduceHP(ratio, VALUE_TYPE.PERCENTAGE);
            }
        }
        else
        {
            lifetime = 0f;
        }
    }
    public override void Disable(PlayerCharacter actor)
    {
        timer = 0f;
    }
}
