using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestProjectile : MonoBehaviour
{
    public static Priest priest;
    Collider2D projectileCollider;
    Rigidbody2D rb;
	public float dano;

    void Start()
    {   
        projectileCollider = GetComponent<Collider2D>();

        if (priest == null)
        {
            priest = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<Priest>();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Walls" || other.transform.tag == "Player")
        {
            gameObject.SetActive(false);
            Projectiles.Projectile projectile = new Projectiles.Projectile(rb, gameObject);
            Priest.projectilePool.Enqueue(projectile);

            if (other.transform.tag == "Player")
            {
		
				if( other.gameObject.GetComponent<Player>().lifeManager.subLife(dano))
					Destroy(other.gameObject);
            }
        }
    }
}
