using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameManager
{
    [Header("Camera")]
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private DirectingCamera directingCamera;

    [Header("Cursor")]
    private CURSOR_MODE cursorMode;
    private IEnumerator slowMotionCoroutine;

    public void Initialize()
    {
        // 해상도
        Screen.SetResolution(Constants.RESOLUTION_DEFAULT_WIDTH, Constants.RESOLUTION_DEFAULT_HEIGHT, true);

        // 커서
        Managers.ResourceManager.LoadResourceAsync<Texture2D>("Sprite_Cursor_Basic", SetCursorTexture);
        SetCursorMode(CURSOR_MODE.UNLOCK);
    }

    public void SaveAndQuit()
    {
        Managers.DataManager.SavePlayerData();
        Application.Quit();
    }

    public IEnumerator SlowMotion(float timeScale, float duration)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    #region Cursor Function
    public void SetCursorTexture(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorMode(CURSOR_MODE cursorMode)
    {
        switch(cursorMode)
        {
            case CURSOR_MODE.LOCK:
                {
                    this.cursorMode = CURSOR_MODE.LOCK;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                }
            case CURSOR_MODE.UNLOCK:
                {
                    this.cursorMode = CURSOR_MODE.UNLOCK;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
        }
    }
    public void ToggleCursorMode()
    {
        if(cursorMode == CURSOR_MODE.LOCK)
        {
            SetCursorMode(CURSOR_MODE.UNLOCK);
        }

        else
        {
            SetCursorMode(CURSOR_MODE.LOCK);
        }
    }
    #endregion

    #region Property
    public PlayerCamera PlayerCamera
    {
        get { return playerCamera; }
        set { playerCamera = value; }
    }
    public DirectingCamera DirectingCamera
    {
        get { return directingCamera; }
        set { directingCamera = value; }
    }
    #endregion
}
