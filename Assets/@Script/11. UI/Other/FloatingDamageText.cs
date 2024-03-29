using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour, IPoolObject
{
    private TextMeshProUGUI damageText;
    private float textSpeed;
    private float duration;
    private Color textColor;

    private IEnumerator autoReturnCoroutine;

    private void Awake()
    {
        TryGetComponent(out damageText);
        textSpeed = 1f;
        duration = 1f;
        textColor = damageText.color;
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, textSpeed * Time.deltaTime, 0));

        textColor.a = Mathf.Lerp(textColor.a, 0, duration * Time.deltaTime);
        damageText.color = textColor;
    }

    public void SetDamageText(bool isCritical, float damage, Vector3 worldPosition)
    {
        transform.position = Managers.GameManager.ActivedCamera.TargetCamera.WorldToScreenPoint(worldPosition);

        if (isCritical == true)
            textColor = Color.red;

        else
            textColor = Color.white;

        damageText.text = damage.ToString("F0");
    }


    public IEnumerator CoAutoReturn()
    {
        yield return new WaitForSeconds(duration);
        ReturnOrDestoryObject();
    }

    #region IPoolObject Interface Fucntion
    public void ActionAfterRequest(ObjectPooler owner)
    {
        ObjectPooler = owner;
        autoReturnCoroutine = CoAutoReturn();

        if (autoReturnCoroutine != null)
            StartCoroutine(autoReturnCoroutine);

        transform.SetParent(Managers.UIManager.UIFixedPanelCanvas.transform);
        textColor.a = 1f;
    }

    public void ActionBeforeReturn()
    {
        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);
    }

    public void ReturnOrDestoryObject()
    {
        if (ObjectPooler == null)
            Destroy(gameObject);

        ObjectPooler.ReturnObject(name, gameObject);
    }

    public ObjectPooler ObjectPooler { get; set; }
    #endregion
}
