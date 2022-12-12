using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotAttackArea : MonoBehaviour
{
    [Header("Dot Attack Area")]
    [SerializeField] private float damageRatio;
    [SerializeField] private float dotTime;
    [SerializeField] private Vector3 boxHalfScale;

    public void OnEnable()
    {
        StartCoroutine(OnDotAttack());
    }
    private void OnDisable()
    {
        StopCoroutine(OnDotAttack());
    }

    private IEnumerator OnDotAttack()
    {
        while(true)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, boxHalfScale);
            for (int i = 0; i < colliders.Length; i++)
            {
                ExecuteDotDamageProcess(colliders[i]);
            }
            yield return new WaitForSeconds(dotTime);
        }
    }

    public void ExecuteDotDamageProcess(Collider target)
    {
        if (target.TryGetComponent(out BaseCharacter character))
        {
            switch (character.IsInvincible)
            {
                case true:
                    {
                        return;
                    }
                case false:
                    {
                        character.StatusData.CurrentHP -= (character.StatusData.MaxHP * damageRatio / 100);
                        return;
                    }
            }
        }
    }
}
