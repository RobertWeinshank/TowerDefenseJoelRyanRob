using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
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
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    public int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps;//Enemies per second
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed); //Anytime onEnemyDestroyed is called, call EnemyDestroyed
    }

    private void Start()
    {
        LevelManager.main.GetWave(currentWave);
        StartCoroutine(StartWave());
    }
    
    private void Update()
    {
        //If we're not spawning, nothing will run in here
        if (!isSpawning) return;
        
        timeSinceLastSpawn += Time.deltaTime; //start the spawn timer

        if(timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0) //if the spawn timer is greater than or equal to the enemies per second spawner and there are enemies left to spawn; spawn enemies and adjust spawn values
        {         
            SpawnEnemy();

            enemiesLeftToSpawn--;
            enemiesAlive++;

            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        LevelManager.main.GetWave(currentWave);
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        //Debug.Log("Spawn Enemy");
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index]; //In the future, we can randomize which enemy spawns
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
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
