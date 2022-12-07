using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAtRayPoint : MonoBehaviour
{
    [SerializeField] protected string objectKey;
    [SerializeField] protected float rayDistance;
    [SerializeField] protected float interval;
    private Vector3 rotationOffset;
    private bool isGeneratable;

    private void OnEnable()
    {
        rotationOffset = new Vector3(-90, 0, 0);
        isGeneratable = true;
        StartCoroutine(GenerateByInterval());
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward.normalized * rayDistance, Color.blue, 0.1f);
        if (Physics.Raycast(transform.position, transform.forward.normalized, out RaycastHit hit, rayDistance, LayerMask.GetMask("Terrain")))
        {
            if (isGeneratable)
            {
                GenerateObject(hit);
                isGeneratable = false;
            }

            return;
        }
    }

    IEnumerator GenerateByInterval()
    {
        while(true)
        {
            if(isGeneratable == false)
            {
                yield return new WaitForSeconds(interval);
                isGeneratable = true;
            }
            yield return null;
        }
    }

    public virtual void GenerateObject(RaycastHit hit)
    {
        DotAttackArea requestObject = Managers.ObjectPoolManager.RequestObject(Constants.RESOURCE_NAME_EFFECT_BLACK_DRAGON_BREATH_AFTER).GetComponent<DotAttackArea>();
        requestObject.transform.position = hit.point;
        requestObject.transform.rotation = Quaternion.Euler(rotationOffset);
    }
}
