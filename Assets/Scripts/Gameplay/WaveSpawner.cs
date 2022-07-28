using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public List<EnemyPrefabs> enemies = new List<EnemyPrefabs>();
    public int currWave;
    public int costEnemyPerWave;
    //public int totalWave;
    public int totalCost;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    private bool collideWithPlayer = false;
    
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];
    private Transform[] transforms;
    private Animator[] animators;

    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    public List<GameObject> spawnedEnemies = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        GenerateWave();
        boxCollider = GetComponent<BoxCollider2D>();
        transforms = this.transform.GetComponentsInChildren<Transform>();
        animators = this.transform.GetComponentsInChildren<Animator>();
    }

    bool CollideWithPlayer()
    {
        bool result = false;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[1] == null)
                continue;

            if (hits[i].tag == "Fighter")
            {
                if (hits[i].name == "Player")
                {
                    result = true;
                }
            }


            // the array is not cleanedup, so we do it ourself
            hits[i] = null;
        }

        return result;
    }

    IEnumerator Spawner()
    {
        // show portal spawner
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool("spawnerShow", true);
        }

        yield return new WaitForSeconds(1);

        if (spawnTimer <= 0)
        {
            //spawn an enemy
            if (enemiesToSpawn.Count > 0)
            {
                GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity); // spawn first enemy in our list
                enemiesToSpawn.RemoveAt(0); // and remove it
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;

                if (spawnIndex + 1 <= spawnLocation.Length - 1)
                {
                    spawnIndex++;
                }
                else
                {
                    spawnIndex = 0;
                }
            }
            else
            {
                waveTimer = 0; // if no enemies remain, end wave
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }

        if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
        {
            if (totalCost > 0)
            {
                currWave++;
                GenerateWave();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check for overlaps
        boxCollider.OverlapCollider(filter, hits);

        if (CollideWithPlayer())
        {
            collideWithPlayer = true;
        }

        if (collideWithPlayer)
        {
            StartCoroutine(Spawner());
        }

        spawnedEnemies.Clear();
    }

    public void GenerateWave()
    {
        waveValue = currWave * costEnemyPerWave;
        GenerateEnemies();
        
        if(enemiesToSpawn.Count > 0)
        {
            spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        }

        waveTimer = waveDuration; // wave duration is read only
    }

    public void GenerateEnemies()
    {
        // Create a temporary list of enemies to generate
        // 
        // in a loop grab a random enemy 
        // see if we can afford it
        // if we can, add it to our list, and deduct the cost.

        // repeat... 

        //  -> if we have no points left, leave the loop

        List<GameObject> generatedEnemies = new List<GameObject>();

        while (waveValue > 0 || generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            totalCost -= randEnemyCost;

            if (totalCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }

        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

}

[System.Serializable]
public class EnemyPrefabs
{
    public GameObject enemyPrefab;
    public int cost;
}