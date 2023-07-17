using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    [Header("Volume")]
    private PostProcessVolume postProcessVolume;

    [Header("Settings")]
    private Bloom bloom;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;

    public void Initialize(GameObject rootObject)
    {
        GameObject postProcessingObject = Functions.FindObjectFromChild(rootObject, "@Post_Processing_Volume");

        if (postProcessingObject == null)
            postProcessingObject = new GameObject("@Post_Processing_Volume");

        postProcessingObject.transform.SetParent(rootObject.transform);
        postProcessVolume = Functions.GetOrAddComponent<PostProcessVolume>(postProcessingObject);
        postProcessVolume.isGlobal = true;
        postProcessVolume.profile = Managers.ResourceManager.LoadResourceSync<PostProcessProfile>("PostProcessing_Mercenary_Default");

        postProcessVolume.profile.TryGetSettings(out bloom);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
        postProcessVolume.profile.TryGetSettings(out vignette);
    }


    private IEnumerator CoChangeLinear(float startValue, float targetValue, float duration = 0.1f)
    {
        float elapsedTime = 0f;
        float currentValue;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }
    }

    public void BeatBloom(float targetValue, float duration)
    {
        StartCoroutine(CoBeatBloom(targetValue, duration));
    }

    private IEnumerator CoBeatBloom(float targetValue, float duration)
    {
        float elapsedTime = 0f;
        float originalValue = bloom.intensity.value;
        float halfDuration = duration * 0.5f;

        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, targetValue, elapsedTime / halfDuration);
            yield return null;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, originalValue, elapsedTime / duration);
            yield return null;
        }
    }

    public void BeatChromaticAberration(float targetValue, float duration)
    {
        StartCoroutine(CoBeatChromaticAberration(targetValue, duration));
    }

    private IEnumerator CoBeatChromaticAberration(float targetValue, float duration)
    {
        float elapsedTime = 0f;
        float originalValue = chromaticAberration.intensity.value;
        float halfDuration = duration * 0.5f;

        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, targetValue, elapsedTime / halfDuration);
            yield return null;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, originalValue, elapsedTime / duration);
            yield return null;
        }
    }
}
