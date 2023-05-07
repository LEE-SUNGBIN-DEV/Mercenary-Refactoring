using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiePopup : UIPopup
{
    public void Initialize()
    {

    }

    public void ReturnLastResonancePoint()
    {
        Managers.SceneManagerCS.LoadSceneAsync(Managers.DataManager.CurrentCharacterData.LocationData.LastScene);
    }
}
