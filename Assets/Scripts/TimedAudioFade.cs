using UnityEngine;
using System.Collections;

public class TimedAudioFade : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource secondAudioSource;
    [SerializeField] private float playDuration = 2f;
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        if (secondAudioSource != null)
        {
            // sound starts playing immediately on Awake/Start
            secondAudioSource.Play();
            StartCoroutine(PlayThenFadeRoutine());
        }
    }

    private IEnumerator PlayThenFadeRoutine()
    {
        // Wait for the initial play duration
        yield return new WaitForSeconds(playDuration);

        // Fade Out
        float startVolume = secondAudioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Linearly interpolate the volume down to 0
            secondAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            yield return null; 
        }

        // stop the audio track
        secondAudioSource.volume = 0f;
        secondAudioSource.Stop();
    }
}
