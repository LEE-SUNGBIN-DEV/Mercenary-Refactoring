using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public void ReturnViliage()
    {
        Managers.UIManager.CommonSceneUI.ConfirmPanel.OnConfirm -= ReturnViliage;
        Managers.SceneManagerCS.LoadSceneAsync(SCENE_LIST.Forestia);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Managers.UIManager.RequestConfirm("������ ���ư��ðڽ��ϱ�?", ReturnViliage);
        }
    }
}
