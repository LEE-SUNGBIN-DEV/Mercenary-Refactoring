using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiePopup : UIPopup
{
    private SCENE_LIST returnScene;

    public void Initialize()
    {

    }
    public void ReturnViliage()
    {
        Managers.SceneManagerCS.LoadSceneAsync(returnScene);
    }

    #region Property
    public SCENE_LIST ReturnScene
    {
        get { return returnScene; }
    }
    #endregion
}
