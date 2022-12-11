using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLightningField : CreateAtRandomLocation
{
    [SerializeField] private BaseEnemy owner;
    private Vector3 offset;

    private void Awake()
    {
        offset = Vector3.zero;
    }

    public override IEnumerator CreateEveryInterval(float interval)
    {
        offset = transform.position;

        for (int i = 0; i < amount; ++i)
        {
            float pointX = Random.Range(-range, range);
            float secondRange = Mathf.Sqrt(range * range - pointX * pointX);
            float pointZ = Random.Range(-secondRange, secondRange);

            GameObject createObject = Managers.ObjectPoolManager.RequestObject(key);
            createObject.transform.position = new Vector3(offset.x + pointX, 0, offset.z + pointZ);
            createObject.GetComponent<EnemyLightningStrike>().Owner = Owner;

            yield return new WaitForSeconds(interval);
        }
    }

    #region Property
    public BaseEnemy Owner
    {
        get { return owner; }
        set
        {
            if (value is BaseEnemy)
            {
                owner = value;
            }
        }
    }
    #endregion
}
