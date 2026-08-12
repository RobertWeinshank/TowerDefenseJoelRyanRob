using UnityEngine;

public class DelayedAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public float delayInSeconds = 2.0f;

    void Start()
    {
        audioSource.PlayDelayed(delayInSeconds); 
    }
}