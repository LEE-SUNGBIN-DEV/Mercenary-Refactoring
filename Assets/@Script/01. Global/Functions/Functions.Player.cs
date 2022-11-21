using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Functions
{
    public static void SetCharacterPosition(Character character, Vector3 targetPosition)
    {
        character.CharacterController.enabled = false;
        character.transform.position = targetPosition;
        character.CharacterController.enabled = true;
    }
    public static void SetCharacterTransform(Character character, Transform targetTransform)
    {
        character.CharacterController.enabled = false;
        character.gameObject.SetTransform(targetTransform);
        character.CharacterController.enabled = true;
    }
    public static Character CreateCharacterWithCamera(Vector3 position)
    {
        GameObject cameraObject = Managers.ResourceManager.InstantiatePrefabSync("Prefab_Player_Camera");
        Character character = Managers.ResourceManager.InstantiatePrefabSync("Prefab_" + Managers.DataManager.SelectCharacterData.StatData.CharacterClass).GetComponent<Character>();

        SetCharacterPosition(character, position);
        cameraObject.transform.position = position;

        return character;
    }
    public static void TeleportCharacterWithCamera(Character character, Vector3 position, PlayerCamera camera)
    {
        SetCharacterPosition(character, position);
        camera.transform.position = position;
    }

    // !! 적의 대미지를 계산하는 함수
    public static void EnemyDamageProcess(Enemy enemy, Character character, float ratio)
    {
        // Damage Process
        float damage = (enemy.AttackPower - character.Status.DefensivePower * 0.5f) * 0.5f;
        if (damage < 0)
        {
            damage = 0;
        }
        damage += ((enemy.AttackPower / 8f - enemy.AttackPower / 16f) + 1f);

        // Final Damage
        damage *= ratio;

        character.Status.CurrentHitPoint -= damage;
    }
}
