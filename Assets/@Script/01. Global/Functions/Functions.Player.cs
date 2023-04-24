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
        Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_PLAYER_CAMERA).TryGetComponent<PlayerCamera>(out PlayerCamera playerCamera);
        if (Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_PLAYER_CHARACTER).TryGetComponent<PlayerCharacter>(out PlayerCharacter player))
        {
            SetCharacterPosition(player, position);
            playerCamera.TargetTransform = player.transform;
        }

        return player;
    }
    public static void WarpCharacterWithCamera(PlayerCharacter character, Vector3 position, PlayerCamera camera)
    {
        SetCharacterPosition(character, position);
        camera.transform.position = position;
    }
}
