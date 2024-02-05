using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TreasureBox : MonoBehaviour, IInteractableObject
{
    [SerializeField] private TreasureBoxData treasureBoxData;
    [SerializeField] private PlayerCharacter targetCharacter;
    [SerializeField] private float distanceFromPlayerCharacter;

    private Animator animator;

    public void Initialize(TreasureBoxData treasureBoxData)
    {
        this.treasureBoxData = treasureBoxData;
        TryGetComponent(out animator);
        if(Managers.DataManager.CurrentCharacterData != null)
        {
            UpdateTreasureBoxState(Managers.DataManager.CurrentCharacterData.SceneData);
        }
    }

    public void UpdateTreasureBoxState(CharacterSceneData sceneData)
    {
        if(sceneData.IsGetTreasureBox(treasureBoxData.treasureBoxID))
        {
            animator.Play("Treasure_Box_Open");
        }
        else
        {
            animator.Play("Treasure_Box_Close");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetCharacter == null && other.TryGetComponent(out PlayerCharacter character) && !character.SceneData.IsGetTreasureBox(treasureBoxData.treasureBoxID))
        {
            targetCharacter = character;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter character) && character == targetCharacter)
        {
            targetCharacter.InteractionController.ExitInteraction(this, targetCharacter);
            targetCharacter = null;
        }
    }

    private void Update()
    {
        if (targetCharacter != null && !targetCharacter.SceneData.IsGetTreasureBox(treasureBoxData.treasureBoxID))
        {
            distanceFromPlayerCharacter = Vector3.SqrMagnitude(targetCharacter.transform.position - transform.position);

            targetCharacter.InteractionController.EnterDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateInteraction(this, targetCharacter);
        }
    }

    // Detection
    public void EnterDetection(PlayerCharacter character)
    {
        if (character.SceneData.IsGetTreasureBox(treasureBoxData.treasureBoxID))
            return;

        Managers.UIManager.UIFixedPanelCanvas.InteractionPanel.OpenPanel(Managers.DataManager.TextTable["TEXT_OPEN"].textContent);
    }
    public void UpdateDetection(PlayerCharacter character)
    {
        if (Managers.InputManager.InteractionEnterButton.WasPressedThisFrame())
        {
            character.InteractionController.EnterInteraction(this, character);
        }
    }
    public void ExitDetection(PlayerCharacter character)
    {
        Managers.UIManager.UIFixedPanelCanvas.InteractionPanel.ClosePanel();
    }

    // Interaction
    public void EnterInteraction(PlayerCharacter character)
    {
        if (character.SceneData.IsGetTreasureBox(treasureBoxData.treasureBoxID))
            return;

        Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.INTERACTION);
        Managers.AudioManager.PlaySFX(Constants.Audio_Treasure_Box_Open);
        animator.Play("Treasure_Box_Open");
        DropItem(character.CharacterData);
        character.SceneData.ModifyTreasureBoxInformation(treasureBoxData.treasureBoxID, true);
        character.InteractionController.ExitInteraction(this, character);
    }
    public void UpdateInteraction(PlayerCharacter character)
    {
    }
    public void ExitInteraction(PlayerCharacter character)
    {
        Managers.InputManager.PopInputMode();
    }

    public void DropItem(CharacterData characterData)
    {
        Functions.DropItem(characterData, treasureBoxData.GetDropData(), treasureBoxData.dropCount);
    }

    public float Distance { get { return distanceFromPlayerCharacter; } }
    public PlayerCharacter TargetCharacter { get { return targetCharacter; } }
}
