using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public static Collider2D playerCollider;
	public float dano;
    Collider2D projectileCollider;
    Rigidbody2D rb;
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
            gameObject.SetActive(false);
            Projectiles.Projectile projectile = new Projectiles.Projectile(rb, gameObject);
            Fire.projectilePool.Enqueue(projectile);
			if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
				if (other.gameObject.GetComponent<LifeManager>().subLife(dano))
				{
                    score.AddScore(100);
                    other.gameObject.GetComponent<Enemy>().Die();
                }
            }
        }   
    }
}
