using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public static Collider2D playerCollider;
	public float dano, chanceDeDropar = 0.75f;
    Collider2D projectileCollider;
    Rigidbody2D rb;
    public GameObject dropSpeed;
    public GameObject dropCooldown;
    ScoreManager score;

    void Start()
    {   
        score = FindObjectOfType<ScoreManager>();
        projectileCollider = GetComponent<Collider2D>();

        if (playerCollider == null)
        {
            playerCollider = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<Collider2D>();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Walls" || 
            other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
			if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
				if (other.gameObject.GetComponent<LifeManager>().subLife(dano))
				{
                    bool rand = Random.Range(0f, 1f) <= chanceDeDropar; 
                    if (rand)
                    {
                        float whichPowerup = Random.Range(0f, 2f);
                        if (whichPowerup <= 1f)
                            Instantiate(dropSpeed,
                                other.gameObject.transform.position, Quaternion.identity);
                        else
                            Instantiate(dropCooldown,
                                other.gameObject.transform.position, Quaternion.identity);
                    }
                    score.AddScore(100);
                    other.gameObject.GetComponent<Enemy>().Die();
                }
            }
            gameObject.SetActive(false);
            Projectiles.Projectile projectile = new Projectiles.Projectile(rb, gameObject);
            Fire.projectilePool.Enqueue(projectile);
        }   
    }
}
