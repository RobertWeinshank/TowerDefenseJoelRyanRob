using UnityEngine;

public class InstructionalPopup : MonoBehaviour
{
    public GameObject instructionPanel;

    void Start()
    {
        // Pause the game on start
        Time.timeScale = 0f;
        if (instructionPanel != null)
            instructionPanel.SetActive(true);
    }

    // Call this function when the UI Button is clicked
    public void StartGame()
    {
        Time.timeScale = 1f;
        if (instructionPanel != null)
            instructionPanel.SetActive(false);
    }
}
