using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static Queue<Projectiles.Projectile> priestPool;
    public GameObject priest;
    public int numPriests = 50;
    
    public float timeBetweenSpawns = 5f;
    float timeSinceLastSpawn;

    int wavesSpawned = 0;
    public int framesBetweenSpawns = 3;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastSpawn = timeBetweenSpawns;
        priestPool = new Queue<Projectiles.Projectile>();
        for (int i = 0; i < numPriests; i++)
        {
            GameObject priestInstance = Instantiate(priest);
            Projectiles.Projectile priestCopy = 
                new Projectiles.Projectile(priestInstance.GetComponent<Rigidbody2D>(),
                priestInstance);
            priestPool.Enqueue(priestCopy);
            priestCopy.instance.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            timeSinceLastSpawn = 0;
            wavesSpawned++;
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
        int num = (int) (wavesSpawned/5f) + 1;
        return num;
    }

    IEnumerator SpawnEnemies(int numEnemies)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            int whichEnemy = (int) Random.Range(0f, 2f);
            Projectiles.Projectile enemyCopy = priestPool.Dequeue();
            enemyCopy.instance.SetActive(true);
            
            enemyCopy.instance.transform.position = transform.position;

            for (int j = 0; j < framesBetweenSpawns; j++)
            {
                yield return null;
            }
        }
    }
}
