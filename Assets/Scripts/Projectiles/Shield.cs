using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static Player player;
    Collider2D projectileCollider;
    Rigidbody2D rb;
    public float dano = 10f;
    bool doDamage = false;

    void Start()
    {   
        projectileCollider = GetComponent<Collider2D>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (doDamage)
        {
            if(player.lifeManager.subLife(dano * Time.fixedDeltaTime))
            {
                // Fim de jogo
                GameController.GameOver();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.transform.tag == "Player")
        {
            doDamage = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "PlayerProjectile")
        {
            Projectiles.Projectile shield = new Projectiles.Projectile(rb, gameObject);
            Monk.shieldPool.Enqueue(shield);
            gameObject.SetActive(false);
            Projectiles.Projectile playerShoot = 
                new Projectiles.Projectile(other.GetComponent<Rigidbody2D>(), other.gameObject);
            Fire.projectilePool.Enqueue(playerShoot);
            playerShoot.instance.SetActive(false);
        }
        else if (other.transform.tag == "Player")
        {
            doDamage = true;
        }
    }
}