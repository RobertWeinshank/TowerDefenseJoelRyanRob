using TMPro;
using UnityEngine;

public class GaiaEnergy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI gaiaEnergyUI;


    //Display health UI
    private void OnGUI()
    {
        gaiaEnergyUI.text = "Gaia Energy: " + LevelManager.main.gaiaEnergy.ToString();
    }
}
