using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fire : MonoBehaviour
{
    public Queue<Projectiles.Projectile> projectilePool =
        new Queue<Projectiles.Projectile>();

    public GameObject projectile;
    public int numProjectiles = 5;
    public float projectileSpeed = 1f;
    public float projectileTimer = 0.25f;
    float lastProjectileTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numProjectiles; i++)
        {
            GameObject projectileInstance = Instantiate(projectile);
            Projectiles.Projectile projectileCopy = 
                new Projectiles.Projectile(projectileInstance.GetComponent<Rigidbody2D>(),
                projectileInstance);
            projectilePool.Enqueue(projectileCopy);
            projectileCopy.instance.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastProjectileTimer > 0)
        {
            lastProjectileTimer -= Time.fixedDeltaTime;
        }
        else
        {
            lastProjectileTimer = projectileTimer;
        }

        if (Input.GetButton("Fire1") && projectilePool.Count > 0 && lastProjectileTimer <= 0f)
        {
            Projectiles.Projectile projectile = projectilePool.Dequeue();
            projectile.instance.SetActive(true);
            projectile.instance.transform.position = transform.position;
            projectile.rb.velocity =
                new Vector3(projectileSpeed * transform.localScale.x, 0f, 0f);
            lastProjectileTimer = lastProjectileTimer;
            Debug.Log("Fired!");
        }
    }
}
