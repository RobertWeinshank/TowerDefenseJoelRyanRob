using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    //Array for ALL TYPES of enemies to spawn in game  
    [SerializeField] private GameObject[] enemyPrefabsWave1;
    [SerializeField] private GameObject[] enemyPrefabsWave2;
    [SerializeField] private GameObject[] enemyPrefabsWave3;
    [SerializeField] private GameObject[] enemyPrefabsWave4;
    [SerializeField] private GameObject[] enemyPrefabsWave5;
    [SerializeField] private GameObject[] enemyPrefabsWave6;
    [SerializeField] private GameObject[] enemyPrefabsWave7;
    [SerializeField] private GameObject[] enemyPrefabsWave8;
    [SerializeField] private GameObject[] enemyPrefabsWave9;
    [SerializeField] private GameObject[] enemyPrefabsWave10;
    [SerializeField] private GameObject[] enemyPrefabsWave11Beyond;
    [SerializeField] private GameObject oilRig;
    [SerializeField] private Plot[] oilRigPlots;
    
    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 10;
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
        //Debug.Log("Spawn Enemy");
        //int index = Random.Range(0, enemyPrefabs.Length);
        //GameObject prefabToSpawn = enemyPrefabs[index]; //In the future, we can randomize which enemy spawns
        //Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation

        if (currentWave == 1)
        {
            Debug.Log("Spawning Wave 1");
            int index = Random.Range(0, enemyPrefabsWave1.Length);
            GameObject prefabToSpawn = enemyPrefabsWave1[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave == 2)
        {
            Debug.Log("Spawning Wave 2");
            int index = Random.Range(0, enemyPrefabsWave2.Length);
            GameObject prefabToSpawn = enemyPrefabsWave2[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave == 3)
        {
            Debug.Log("Spawning Wave 3");
            int index = Random.Range(0, enemyPrefabsWave3.Length);
            GameObject prefabToSpawn = enemyPrefabsWave3[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave == 4)
        {
            Debug.Log("Spawning Wave 4");
            int index = Random.Range(0, enemyPrefabsWave4.Length);
            GameObject prefabToSpawn = enemyPrefabsWave4[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave == 5)
        {
            Debug.Log("Spawning Wave 5");
            int index = Random.Range(0, enemyPrefabsWave5.Length);
            GameObject prefabToSpawn = enemyPrefabsWave5[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave == 6)
        {
            Debug.Log("Spawning Wave 6");
            int index = Random.Range(0, enemyPrefabsWave6.Length);
            GameObject prefabToSpawn = enemyPrefabsWave6[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave == 7)
        {
            Debug.Log("Spawning Wave 7");
            int index = Random.Range(0, enemyPrefabsWave7.Length);
            GameObject prefabToSpawn = enemyPrefabsWave7[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave == 8)
        {
            Debug.Log("Spawning Wave 8");
            int index = Random.Range(0, enemyPrefabsWave8.Length);
            GameObject prefabToSpawn = enemyPrefabsWave8[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave == 9)
        {
            Debug.Log("Spawning Wave 9");
            int index = Random.Range(0, enemyPrefabsWave9.Length);
            GameObject prefabToSpawn = enemyPrefabsWave9[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave == 10)
        {
            Debug.Log("Spawning Wave 10");
            int index = Random.Range(0, enemyPrefabsWave10.Length);
            GameObject prefabToSpawn = enemyPrefabsWave10[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
        else if (currentWave >= 11)
        {
            Debug.Log("Spawning Wave 11+");
            int index = Random.Range(0, enemyPrefabsWave11Beyond.Length);
            GameObject prefabToSpawn = enemyPrefabsWave11Beyond[index]; //In the future, we can randomize which enemy spawns
            Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity); //Spawn the prefab, at the starting point, at its current rotation
        }
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
