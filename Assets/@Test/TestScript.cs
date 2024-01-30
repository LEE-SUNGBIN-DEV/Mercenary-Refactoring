using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        Collider[] nearbyAgentColliders = Physics.OverlapSphere(transform.position, 2, 1 << Constants.LAYER_ENEMY);
        for (int i = 0; i < nearbyAgentColliders.Length; ++i)
        {
            if (nearbyAgentColliders[i] != GetComponent<Collider>())
            {
                Debug.Log($"{nearbyAgentColliders[i].name} Added");
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
