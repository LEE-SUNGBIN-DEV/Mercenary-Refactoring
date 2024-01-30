using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseGateWarp : MonoBehaviour
{
    [SerializeField] private Collider warpCollider;
    private ResponseGateData responseGateData;

    private void Awake()
    {
        if(TryGetComponent(out warpCollider))
        {
            Debug.LogWarning($"Warning: {gameObject.name} has not warpCollider");
        }
    }

    public void Initialize(ResponseGateData responseGateData)
    {
        this.responseGateData = responseGateData;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Warp
        WarpTargetGate();
    }

    public void WarpTargetGate()
    {
        // Play SFX
        Managers.DataManager.CurrentCharacterData.LocationData.SetLastResponseGate(responseGateData.destinationGateID);
        Managers.SceneManagerEX.LoadSceneAsync(responseGateData.GetDestinationScene());
    }
}
