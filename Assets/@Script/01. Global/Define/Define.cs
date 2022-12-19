using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot
{
    public int slotIndex;
    public SelectionCharacter selectionCharacter;
    public Vector3 characterPoint;
    public TextMeshProUGUI slotText;
    public Button slotButton;

    public CharacterSlot()
    {
        slotText = null;
        slotButton = null;
    }
}

[System.Serializable]
public class MaterialContainer
{
    public string key;
    public Material value;
}



