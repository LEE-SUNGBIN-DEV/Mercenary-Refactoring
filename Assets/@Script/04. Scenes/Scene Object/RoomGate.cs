using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGate : MonoBehaviour
{
    private Animator animator;
    private int openHash;
    private int closeHash;

    public void Initialize()
    {
        if (TryGetComponent(out animator))
        {
            openHash = Animator.StringToHash("Open");
            closeHash = Animator.StringToHash("Close");
        }
    }

    public void OpenGate()
    {
        animator.Play(openHash);
    }

    public void CloseGate()
    {
        animator.Play(closeHash);
    }
}
