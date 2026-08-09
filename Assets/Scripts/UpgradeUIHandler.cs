using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private GameObject upgradeUI;

    public bool mouse_over = false;
    public bool turnOff = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (upgradeUI == null) return;

        mouse_over = true;
        UIManager.main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (upgradeUI == null) return;

        mouse_over = false;
        UIManager.main.SetHoveringState(false);
        upgradeUI.SetActive(false);
    }
}
