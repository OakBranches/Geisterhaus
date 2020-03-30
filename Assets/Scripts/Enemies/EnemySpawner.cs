using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static Queue<Projectiles.Projectile> priestPool;
    public static Queue<Projectiles.Projectile> bishopPool;
    public static Queue<Projectiles.Projectile> monkPool;
    public GameObject priest;
    public GameObject bishop;
    public GameObject monk;
    public int numPriests = 50;
    public int numBishops = 50;
    public int numMonks = 50;
    
    public float timeBetweenSpawns = 5f;
    float fixedTimeBetweenSpawns;
    float timeSinceLastSpawn;

    int wavesSpawned = 0;
    public int framesBetweenSpawns = 3;

    // Start is called before the first frame update
    void Start()
    {
        fixedTimeBetweenSpawns = timeBetweenSpawns;
        ScoreManager.startedPlaying = true;
        timeSinceLastSpawn = timeBetweenSpawns;
        priestPool = new Queue<Projectiles.Projectile>();
        bishopPool = new Queue<Projectiles.Projectile>();
        monkPool = new Queue<Projectiles.Projectile>();

        for (int i = 0; i < numPriests; i++)
        {
            GameObject priestInstance = Instantiate(priest);
            Projectiles.Projectile priestCopy = 
                new Projectiles.Projectile(priestInstance.GetComponent<Rigidbody2D>(),
                priestInstance);
            priestPool.Enqueue(priestCopy);
            priestCopy.instance.SetActive(false);
        }

        for (int i = 0; i < numBishops; i++)
        {
            GameObject bishopInstance = Instantiate(bishop);
            Projectiles.Projectile bishopCopy = 
                new Projectiles.Projectile(bishopInstance.GetComponent<Rigidbody2D>(),
                bishopInstance);
            bishopPool.Enqueue(bishopCopy);
            bishopCopy.instance.SetActive(false);
        }

        for (int i = 0; i < numMonks; i++)
        {
            GameObject monkInstance = Instantiate(monk);
            Projectiles.Projectile monkCopy = 
                new Projectiles.Projectile(monkInstance.GetComponent<Rigidbody2D>(),
                monkInstance);
            monkPool.Enqueue(monkCopy);
            monkCopy.instance.SetActive(false);
        }

        StartCoroutine(SpawnEnemies(5));
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            timeSinceLastSpawn = 0;
            wavesSpawned++;
            timeBetweenSpawns = CalculateTimeBetweenSpawns();
            int enemiesToSpawn = EnemiesPerWave();
            StartCoroutine(SpawnEnemies(enemiesToSpawn));
        }
        else
        {
            timeSinceLastSpawn += Time.deltaTime;
        }
    }

    int EnemiesPerWave()
    {
        // f(x) = a * x + b
        int num = (int) (wavesSpawned/10f) + 1;
        return num;
    }

    IEnumerator SpawnEnemies(int numEnemies)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            Projectiles.Projectile enemyCopy;
            if (wavesSpawned < 10)
            {
                enemyCopy = priestPool.Dequeue();
            }
            else if (wavesSpawned < 20)
            {
                if (i == 0)
                {
                    enemyCopy = priestPool.Dequeue();
                }
                else
                {
                    float whichEnemy = Random.Range(0f, 2f);
                    if (whichEnemy < 1f)
                    {
                        enemyCopy = priestPool.Dequeue();
                    }
                    else
                    {
                        enemyCopy = bishopPool.Dequeue();
                    }
                }
            }
            else
            {
                if (i == 0)
                {
                    enemyCopy = monkPool.Dequeue();
                }
                else
                {
                    float whichEnemy = Random.Range(0f, 3f);
                    if (whichEnemy < 1f) 
                    {
                        enemyCopy = priestPool.Dequeue();
                    }
                    else if (whichEnemy < 2f) 
                    {
                        enemyCopy = bishopPool.Dequeue();
                    }
                    else
                    {
                        enemyCopy = monkPool.Dequeue();
                    }
                }
            }
            enemyCopy.instance.SetActive(true);
            
            enemyCopy.instance.transform.position = transform.position;

            for (int j = 0; j < framesBetweenSpawns; j++)
            {
                yield return null;
            }
        }
    }

    float CalculateTimeBetweenSpawns()
    {
        // f(x) = a * x + b
        return (float) fixedTimeBetweenSpawns + wavesSpawned / 5f;
    }
}
