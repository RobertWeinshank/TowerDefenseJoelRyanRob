using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SecondaryEnemySpawner : MonoBehaviour
{
    [Header("References")]
    //Array for ALL TYPES of enemies to spawn in game  
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Events")]
    public static UnityEvent onSecondaryEnemyDestroy = new UnityEvent();

    public int currentWave = 5;
    private float timeSinceLastSpawn;
    private int secondaryEnemiesAlive;
    private int secondaryEnemiesLeftToSpawn;
    private float eps;//Enemies per second
    private bool isSpawning = false;

    private void Awake()
    {
        onSecondaryEnemyDestroy.AddListener(EnemyDestroyed); //Anytime onEnemyDestroyed is called, call EnemyDestroyed
    }

    private void Start()
    {
        //LevelManager.main.GetSecondWave(currentWave);
        StartCoroutine(StartSecondWave());
    }

    private void Update()
    {
        if (LevelManager.main.currentWave >= 3)
        {
            //If we're not spawning, nothing will run in here
            if (!isSpawning) return;

            timeSinceLastSpawn += Time.deltaTime; //start the spawn timer

            if (timeSinceLastSpawn >= (1f / eps) && secondaryEnemiesLeftToSpawn > 0) //if the spawn timer is greater than or equal to the enemies per second spawner and there are enemies left to spawn; spawn enemies and adjust spawn values
            {
                SpawnEnemy();

                secondaryEnemiesLeftToSpawn--;
                secondaryEnemiesAlive++;

                timeSinceLastSpawn = 0f;
            }

            if (secondaryEnemiesAlive == 0 && secondaryEnemiesLeftToSpawn == 0)
            {
                EndWave();
            }
        }

    }

    private void EnemyDestroyed()
    {
        secondaryEnemiesAlive--;
    }

    private IEnumerator StartSecondWave()
    {
        Debug.Log("Start Secondary Wave");
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        secondaryEnemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        LevelManager.main.GetSecondWave(currentWave);
        StartCoroutine(StartSecondWave());
    }

    private void SpawnEnemy()
    {
        //Debug.Log("Spawn Secondary Enemy");
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index]; //In the future, we can randomize which enemy spawns
        Instantiate(prefabToSpawn, LevelManager.main.secondaryStartPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor)); //Round the baseEnemies * currentWave^difficultyScalingFactor (8*1^.75 =8; 8*2^.75 =14;8*10^.75=45) 
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0, enemiesPerSecondCap);
    }
}
