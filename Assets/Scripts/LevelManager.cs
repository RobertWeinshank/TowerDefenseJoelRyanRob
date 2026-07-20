using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform secondaryStartPoint;
    //Enemy path that they take (The orange points in the game)
    public Transform[] path;
    public Transform[] secondaryPath;

    public int currency;
    public int health;
    public int gaiaEnergy;
    public int currentWave = 1;
    public int currentSecondWave = 5;

    private float energyCounter;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 100;
        health = 100;
        gaiaEnergy = 0;
    }

    private void Update()
    {
        energyCounter += Time.deltaTime;

        if (energyCounter >= 3f)
        {
            gaiaEnergy += 1; //increases the gaiaEnergy counter by 1 every 3 seconds
            energyCounter = 0;
        }
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            //Debug.Log("Youre a brokee,");
            return false; //do nothing if the player is broke (can add a small ui to display it later)
        }
    }

    public void ChangeHealth(int amount)
    {
        health -= amount;
    }

    public void IncreaseGaiaEnergy(int amount)
    {
        gaiaEnergy += amount;
    }

    public void GetWave(int amount) // get reference from EnemySpawner on wave count
    {
        currentWave = amount;
    }
    public void GetSecondWave(int amount) // get reference from EnemySpawner on wave count
    {
        currentSecondWave = amount;
    }
}
