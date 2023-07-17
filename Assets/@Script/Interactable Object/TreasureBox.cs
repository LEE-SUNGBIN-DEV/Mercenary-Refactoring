using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TreasureBox : MonoBehaviour, IInteractable
{
    [SerializeField] private TreasureBoxData treasureBoxData;
    [SerializeField] private PlayerCharacter targetCharacter;
    [SerializeField] private float distanceFromTarget;

    private Animator animator;
    private bool isAvailable;

    public void Initialize(TreasureBoxData treasureBoxData)
    {
        this.treasureBoxData = treasureBoxData;
        TryGetComponent(out animator);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetCharacter == null && other.TryGetComponent(out PlayerCharacter character) && !character.SceneData.IsGetTreasureBox(treasureBoxData.id))
        {
            targetCharacter = character;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter character) && character == targetCharacter)
        {
            targetCharacter.InteractionController.InactiveDetection(this, targetCharacter);
            targetCharacter.InteractionController.InactiveInteraction(this, targetCharacter);
            targetCharacter = null;
        }
    }

    private void Update()
    {
        if (targetCharacter != null && !targetCharacter.SceneData.IsGetTreasureBox(treasureBoxData.id))
        {
            distanceFromTarget = Vector3.SqrMagnitude(targetCharacter.transform.position - transform.position);

            targetCharacter.InteractionController.ActiveDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateInteraction(this, targetCharacter);
        }
    }

    // Detection
    public void EnableDetection(PlayerCharacter character)
    {
        if (character.SceneData.IsGetTreasureBox(treasureBoxData.id))
            return;

        Managers.UIManager.OpenPanel(Managers.UIManager.GameSceneUI.InteractionPanel);
    }
    public void UpdateDetection(PlayerCharacter character)
    {
    }
    public void DisableDetection(PlayerCharacter character)
    {
        Managers.UIManager.ClosePanel(Managers.UIManager.GameSceneUI.InteractionPanel);
    }


    // Interaction
    public void EnableInteraction(PlayerCharacter character)
    {
        if (character.SceneData.IsGetTreasureBox(treasureBoxData.id))
            return;

        character.SceneData.ModifyTreasureBoxInformation(treasureBoxData.id, true);
        animator.Play("Treasure_Box_Open");
        isAvailable = false;
    }
    public void UpdateInteraction(PlayerCharacter character)
    {
    }

    public void DisableInteraction(PlayerCharacter character)
    {
    }

    public float DistanceFromTarget { get { return distanceFromTarget; } }
    public PlayerCharacter TargetCharacter { get { return targetCharacter; } }
}
