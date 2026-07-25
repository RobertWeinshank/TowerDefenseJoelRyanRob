using UnityEngine;

public class TreeHugger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private UnityEngine.UI.Button upgradeButton;


    [Header("Attribute")]
    [SerializeField] private int healthRestored = 5;
    [SerializeField] private float healthPerSecond = 1.0f;
    [SerializeField] private int baseUpgradeCost = 100; //upgrade stuff

    //upgrade stuff
    private float healthPerSecondBase;
    private int level = 1;
    void Start()
    {
        LevelManager.main.ChangeHealth(-healthRestored);
        //need to change it so health can't go past 100 hp max (or however high we want it).
        //Upgrade stuff
        healthPerSecondBase = healthPerSecond;
        upgradeButton.onClick.AddListener(Upgrade); //anytime you click the upgrade button, calls the upgrade method
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;

        healthPerSecond = CalculateHealthPerSecond();

        CloseUpgradeUI();
        Debug.Log("New BPS: " + healthPerSecond + "\nNew Cost: " + CalculateCost());
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateHealthPerSecond()
    {
        return healthPerSecondBase * Mathf.Pow(level, 0.6f);
    }

}
