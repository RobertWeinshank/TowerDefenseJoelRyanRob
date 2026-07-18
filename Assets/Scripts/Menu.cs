using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;


    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen; //Change menu toggle on button press
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    //Display currency UI
    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }
}
