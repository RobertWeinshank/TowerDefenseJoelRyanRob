using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI healthUI;


    //Display health UI
    private void OnGUI()
    {
        healthUI.text = "Health: " + LevelManager.main.health.ToString();
    }
}
