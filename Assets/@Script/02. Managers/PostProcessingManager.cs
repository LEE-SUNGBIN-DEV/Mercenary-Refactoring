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

    [Header("Customs")]
    private ScreenShockWave screenShockWave;

    private Coroutine shockWaveCoroutine;

    public void Initialize(GameObject rootObject)
    {
        GameObject postProcessingObject = Functions.FindObjectFromChild(rootObject, "@POST_PROCESSING_VOLUME");

        if (postProcessingObject == null)
            postProcessingObject = new GameObject("@POST_PROCESSING_VOLUME");

        postProcessingObject.transform.SetParent(rootObject.transform);
        postProcessVolume = Functions.GetOrAddComponent<PostProcessVolume>(postProcessingObject);
        postProcessVolume.isGlobal = true;
        postProcessVolume.profile = Managers.ResourceManager.LoadResourceSync<PostProcessProfile>("POST_PROCESS_PROFILE_DEFAULT");

        postProcessVolume.profile.TryGetSettings(out bloom);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
        postProcessVolume.profile.TryGetSettings(out vignette);
    }

    #region Built-in Post Process
    public void BeatBloom(float targetValue, float duration)
    {
        StartCoroutine(CoBeatBloom(targetValue, duration));
    }
    public void BeatChromaticAberration(float targetValue, float duration)
    {
        StartCoroutine(CoBeatChromaticAberration(targetValue, duration));
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
    #endregion
    #region Custom Post Process
    public void EnableShockWave(float duration = 1f)
    {
        if (shockWaveCoroutine != null)
            StopCoroutine(shockWaveCoroutine);

        shockWaveCoroutine = StartCoroutine(CoEnableShockWave(duration));
    }
    public void DisableShockWave()
    {
        if (shockWaveCoroutine != null)
            StopCoroutine(shockWaveCoroutine);
        screenShockWave.enabled = false;
    }
    private IEnumerator CoEnableShockWave(float duration)
    {
        screenShockWave.enabled = true;
        yield return new WaitForSeconds(duration);
        screenShockWave.enabled = false;
    }
    #endregion

    public ScreenShockWave ScreenShockWave { get { return screenShockWave; } set { screenShockWave = value; } }
}
