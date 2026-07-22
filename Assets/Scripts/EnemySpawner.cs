using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    //Array for ALL TYPES of enemies to spawn in game  
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject oilRig;
    [SerializeField] private Plot[] oilRigPlots;
    
    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;
    [SerializeField] private LevelManager.PathType pathType = LevelManager.PathType.Primary;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    public int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps;//Enemies per second
    private bool isSpawning = false;
    private bool spawnOilJack = true;
    public int plotLocation = 0;

    private GameObject tower;

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
        Debug.Log("Enemies left to spawn: " + enemiesLeftToSpawn + "\nEnemies Alive: " + enemiesAlive);
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

        //if (currentWave == 2 && spawnOilJack)
        //{
        //    spawnOilJack = false;
        //    //CheckOilRigLocation();
        //    SpawningOilJack();
        //    plotLocation++;
        //}

        //if (currentWave == 3 && !spawnOilJack)
        //{
        //    spawnOilJack = true;
        //}

        //if (currentWave == 4 && spawnOilJack)
        //{
        //    spawnOilJack = false;
        //    //CheckOilRigLocation();
        //    SpawningOilJack();
        //    plotLocation++;
        //}

        //if (currentWave == 5 && !spawnOilJack)
        //{
        //    spawnOilJack = true;
        //}

        //if (currentWave == 6 && spawnOilJack)
        //{
        //    spawnOilJack = false;
        //    //CheckOilRigLocation();
        //    SpawningOilJack();
        //}   
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
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index]; //In the future, we can randomize which enemy spawns
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        EnemyMovement enemyMovement = prefabToSpawn.GetComponent<EnemyMovement>();
        enemyMovement.SetPathType(pathType);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor)); //Round the baseEnemies * currentWave^difficultyScalingFactor (8*1^.75 =8; 8*2^.75 =14;8*10^.75=45) 
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0, enemiesPerSecondCap);
    }
    
    private void SpawningOilJack()
    {
        Instantiate(oilRig, oilRigPlots[plotLocation].getPlot(), Quaternion.identity);
    }

    private int CheckOilRigLocation()
    {
        for (int i = 0; i <= oilRigPlots.Length; i++)
        {
            if (tower != oilRigPlots[i])
            {
                plotLocation = i;
                return plotLocation;
            }
            
        }  
        
        return -1;
    }
}
