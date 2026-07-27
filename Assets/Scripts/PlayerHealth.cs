using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI healthUI;

    [Header("Flash Settings")]
    [SerializeField] Color flashColor = Color.red;
    [SerializeField] float flashDuration = 1f; // Increased from 0.2f for a slower fade

    [Header("Scale Settings")]
    [SerializeField] Vector3 targetScale = new Vector3(1.3f, 1.3f, 1.3f); // Scales up to 130%

    private float previousHealth;
    private Color originalColor;
    private Vector3 originalScale;
    private Coroutine flashCoroutine;

    private void Start()
    {
        if (LevelManager.main != null)
        {
            previousHealth = LevelManager.main.health;
        }
        if (healthUI != null)
        {
            originalColor = healthUI.color;
            originalScale = healthUI.transform.localScale;
        }
    }

    private void Update()
    {
        if (LevelManager.main == null || healthUI == null) return;

        float currentHealth = LevelManager.main.health;

        if (currentHealth < previousHealth)
        {
            TriggerFlash();
        }

        previousHealth = currentHealth;
        healthUI.text = "Health: " + currentHealth.ToString();
    }

    private void TriggerFlash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashTextRoutine());
    }

    private IEnumerator FlashTextRoutine()
    {
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / flashDuration;

            // Lerp color and scale simultaneously
            healthUI.color = Color.Lerp(flashColor, originalColor, normalizedTime);
            healthUI.transform.localScale = Vector3.Lerp(targetScale, originalScale, normalizedTime);

            yield return null;
        }

        // Reset to exact starting values
        healthUI.color = originalColor;
        healthUI.transform.localScale = originalScale;
        flashCoroutine = null;
    }
}
