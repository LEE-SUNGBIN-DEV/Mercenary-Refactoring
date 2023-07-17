using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Functions
{
    public static void SetCharacterPosition(PlayerCharacter character, Vector3 targetPosition)
    {
        character.transform.position = targetPosition;
    }
    public static void SetCharacterTransform(PlayerCharacter character, Transform targetTransform)
    {
        character.gameObject.SetTransform(targetTransform);
    }

    public static void WarpCharacterWithCamera(PlayerCharacter character, Vector3 position, PlayerCamera camera)
    {
        SetCharacterPosition(character, position);
        camera.transform.position = position;
    }
}
