using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Functions
{
    public static void SetCharacterPosition(PlayerCharacter character, Vector3 targetPosition)
    {
        character.CharacterController.enabled = false;
        character.transform.position = targetPosition;
        character.CharacterController.enabled = true;
    }
    public static void SetCharacterTransform(PlayerCharacter character, Transform targetTransform)
    {
        character.CharacterController.enabled = false;
        character.gameObject.SetTransform(targetTransform);
        character.CharacterController.enabled = true;
    }
    public static PlayerCharacter CreateCharacterWithCamera(Vector3 position)
    {
        GameObject cameraObject = Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_Player_Camera);
        PlayerCharacter character = Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_Player_Character).GetComponent<PlayerCharacter>();

        SetCharacterPosition(character, position);
        cameraObject.transform.position = position;

        return character;
    }
    public static void TeleportCharacterWithCamera(PlayerCharacter character, Vector3 position, PlayerCamera camera)
    {
        SetCharacterPosition(character, position);
        camera.transform.position = position;
    }
}
