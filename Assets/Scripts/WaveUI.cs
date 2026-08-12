using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI waveUI;

    //Display health UI
    private void OnGUI()
    {
        waveUI.text = "Wave " + LevelManager.main.currentWave.ToString();
    }
}
