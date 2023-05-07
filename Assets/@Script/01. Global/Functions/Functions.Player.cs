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

    public static void WarpCharacterWithCamera(PlayerCharacter character, Vector3 position, PlayerCamera camera)
    {
        SetCharacterPosition(character, position);
        camera.transform.position = position;
    }
}
