using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounterableController : MonoBehaviour
{
    [SerializeField] private Enemy owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Attack"))
        {
            LancerWeapon playerAttack = other.GetComponent<LancerWeapon>();
            if (playerAttack != null && playerAttack.CombatType == COMBAT_TYPE.PlayerCounterAttack)
            {
                Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_COMPETE_START, other.bounds.ClosestPoint(transform.position));
                Owner.OnStun();
                Owner.MeshRenderer.material.color = Color.white;
                gameObject.SetActive(false);
            }
        }
    }

    #region Property
    public Enemy Owner
    {
        get { return owner; }
        set { owner = value; }
    }
    #endregion
}
