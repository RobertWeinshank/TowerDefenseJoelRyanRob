using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TextFadeIn : MonoBehaviour
{
    [Header("Fade Timing Layout (Must equal Activation Clip duration)")]
    [Range(0f, 8f)] [SerializeField] private float fadeInDuration = 1f;
    [Range(0f, 36f)] [SerializeField] private float holdDuration = 6f;
    [Range(0f, 8f)] [SerializeField] private float fadeOutDuration = 1f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeSequenceRoutine());
    }

    private IEnumerator FadeSequenceRoutine()
    {
        float elapsedTime = 0f;

        // Fade In
        if (fadeInDuration > 0f)
        {
            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
                yield return null;
            }
        }
        canvasGroup.alpha = 1f;

        // Hold Visible
        yield return new WaitForSeconds(holdDuration);

        // Fade Out
        elapsedTime = 0f;
        if (fadeOutDuration > 0f)
        {
            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeOutDuration));
                yield return null;
            }
        }
        canvasGroup.alpha = 0f;
    }
}
