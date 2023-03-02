using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDurationState
{
    public float Duration { get;}
    public void SetDuration(float duration = 0f);
}
