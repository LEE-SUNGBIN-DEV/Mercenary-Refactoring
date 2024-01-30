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

    public static void DropItem(CharacterData characterData, DropTableData dropData, int dropCount = 1)
    {
        if (characterData != null && dropData != null)
        {
            for (int i = 0; i < dropCount; ++i)
            {
                characterData.StatusData.RewardExperience(dropData.dropExperience);
                characterData.InventoryData.RewardResponseStone(dropData.dropResonanceStone);
                characterData.InventoryData.RewardItem(dropData.DropItem());
            }
        }
    }
}
