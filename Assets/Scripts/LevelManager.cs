using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform secondaryStartPoint;
    //Enemy path that they take (The orange points in the game)
    public Transform[] path;
    public Transform[] secondaryPath;
    public Transform seedStartPoint;

    public GameObject[] quadrant1;
    public GameObject[] quadrant2;
    public GameObject[] quadrant3;

    public GameObject[] riverTiles;

    public GameObject smog;
    public GameObject smog1;
    public GameObject smog2;


    public int currency;
    public int health;
    public int gaiaEnergy;
    public int currentWave = 1;
    public int currentSecondWave = 5;

    private float energyCounter;
    private bool isGameOver = false; // Prevents the game over logic from running multiple time

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 600;
        health = 300;
        gaiaEnergy = 0;

        
    }

    private void Update()
    {
        if (isGameOver) return; // Stops energy production if the game is over

        energyCounter += Time.deltaTime;

        if (energyCounter >= 3f)
        {
            gaiaEnergy += 1; //increases the gaiaEnergy counter by 1 every 3 seconds
            energyCounter = 0;
        }

        if (currentWave == 2)
        {
            for (int i = 0; i < quadrant1.Length; i++)
            {
                quadrant1[i].SetActive(true);
            }
            smog.SetActive(false);
        }

        if (currentWave == 4)
        {
            
            for (int i = 0; i < quadrant2.Length; i++)
            {
                quadrant2[i].SetActive(true);
            }
            smog1.SetActive(false);

            for (int i = 0; i < riverTiles.Length; i++)
            {
                riverTiles[i].SetActive(false);
            }
        }

        if (currentWave == 6)
        {
            for (int i = 0; i < quadrant3.Length; i++)
            {
                quadrant3[i].SetActive(true);
            }
            smog2.SetActive(false);
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
        if (isGameOver) return; //Ignore damage if already dead
        health -= amount;

        // Check if health has dropped to or below zero
        if (health <= 0)
        {
            health = 0; // Keep health from showing negative numbers in UI
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! The player has lost.");
        SceneManager.LoadScene("DeathScene");
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
