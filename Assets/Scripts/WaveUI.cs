using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI waveUI;

    //Display health UI
    private void OnGUI()
    {
        waveUI.text = LevelManager.main.currentWave.ToString();
    }
}
