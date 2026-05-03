using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; // a list of the groups of enemies to spawn in this wave
        public int waveQuota; // total number of enemies to spawn in this wave
        public float spawnInterval; // interval at which to spawn enemies
        public int spawnCount; // number of enemies already spawned in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; // number of enemies to spawn in this wave
        public int spawnCount; // number of enemies of this type already spawned in this wave
        public GameObject enemyPrefab;
    }
    public List<Wave> waves; // a list of all the waves in the game
    public int currentWaveCount; // the index of the current wave [lists start from 0]

    [Header("Spawner Attributes")]
    float spawnTimer; //timer used to determine when to spawn the next enemy
    public int enemiesAlive; // track the amount of enemies on the field
    public int maxEnemiesAllowed; // max number of enemies allowed on the map at once
    public bool maxEnemiesReached = false; // a flag indicating if the max number of enemies has been reached
    public float waveInterval; // the interval between each wave
    bool isWaveActive = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; // a list that stores all the relative spawn points of enemies

    public AudioSource bgmSource;
    public AudioClip normalBGM;
    public AudioClip finalWaveBGM;

    Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaveCount >= waves.Count) return;

        Wave currentWave = waves[currentWaveCount];

        // Only spawn while this wave still has enemies left to spawn
        if (currentWave.spawnCount < currentWave.waveQuota)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= currentWave.spawnInterval)
            {
                spawnTimer = 0f;
                SpawnEnemies();
            }
        }

        // Only begin next wave after:
        // 1. all enemies for this wave have spawned
        // 2. all enemies from this wave are dead
        // 3. we are not already waiting for the next wave
        if (
            currentWave.spawnCount >= currentWave.waveQuota &&
            enemiesAlive <= 0 &&
            !isWaveActive
        )
        {
            StartCoroutine(BeginNextWave());
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive = true;

        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
            spawnTimer = 0f;
            isWaveActive = false;

            // If this is the LAST wave
            if (currentWaveCount == waves.Count - 1)
            {
                bgmSource.clip = finalWaveBGM;
                bgmSource.Play();
            }
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
    }

    /// <summary>
    /// this method will stop spawning enemies if the amount of enemies on the map is at its maximum.
    /// the method will only spawn enemies in a particular wave until it is time for the next wave's enemies to be spawned
    /// </summary>

    void SpawnEnemies()
    {
        // check if the minimum numbe rof enemies in the wave have been spawned
        if(waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            // spawn each type of enemy until quota is filled
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // check if the minimum number of enemies of this type have spawned
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    // limit the number of enemies that can be spawned at once
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                    //spawn the enemy at a random position close to the player
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        // reset the maxenemiesreached flag if the number of enemies alive has dropped below the max amount
        if(enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    // call after enemy is killed
    public void OnEnemyKilled()
    {
        // decrement number of enemies alive
        enemiesAlive--;
    }
}
