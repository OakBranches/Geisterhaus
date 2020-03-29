using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NPControl))]
[RequireComponent(typeof(Light2D))]
public class Bishop : Enemy
{
    Animator animator;
    BoxCollider2D boxCollider;
    Rigidbody2D rb;
    NPControl control;
    Light2D chargeLight;

    public float verticalSpeed = 5f;
    public Vector2 shootPosition = Vector2.zero;
    Vector2 globalShootPosition;
	public LifeManager lifeManager;
    public GameObject laser;
    public static Queue<Projectiles.Projectile> laserPool;
    Queue<Projectiles.Projectile> laserQueue;
    public float attackTimer = 2.5f;
    float timeSinceLastAttack = 0f;
    public int maxLaserLength = 1000;
    public int framesBeforeLaser = 4;
    public int laserDurationFrames = 15;
    bool isAttacking = false;

    public LayerMask laserCollisionMask;

    public override void AttackMove(float speed, Vector3 toWhere)
    {
        if (!isAttacking)
        {
            float step = Time.deltaTime * speed;
            float directionX;

            toWhere.x = player.transform.position.x;

            Vector3 velocity = new Vector3(speed, 0f, 0f);
            directionX = toWhere.x - transform.position.x;
            if (directionX < -0.05)
            {
                velocity.x *= -1f;
            }

            if (-0.05f < directionX && directionX < 0.05f)
            {
                velocity.x = 0f;
            }

            if ((velocity.x < 0f && control.facingRight) ||
                (velocity.x > 0f && !control.facingRight))
            {
                control.Flip();
            }

            Vector3 targetPosition = new Vector3(toWhere.x, 
                transform.position.y + rb.velocity.y * Time.fixedDeltaTime, 0f);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

            if (velocity.x != 0)
            {
                control.animator.SetBool("walking", true);
            }
            else
            {
                control.animator.SetBool("walking", false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<NPControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackMode = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        lifeManager = GetComponent<LifeManager>();
        chargeLight = GetComponentInChildren<Light2D>();

        if (laserPool == null)
        {
            laserPool = new Queue<Projectiles.Projectile>();
        }
        
        for (int i = 0; i < maxLaserLength; i++)
        {
            GameObject laserPart = Instantiate(laser);
            Projectiles.Projectile laserCopy = 
                new Projectiles.Projectile(laserPart.GetComponent<Rigidbody2D>(),
                laserPart);
            laserPool.Enqueue(laserCopy);
            laserCopy.instance.SetActive(false);
        }

        chargeLight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        globalShootPosition = shootPosition +
            new Vector2(transform.position.x, transform.position.y);
        if (attackMode)
        {
            if (timeSinceLastAttack > 0)
            {
                timeSinceLastAttack -= Time.fixedDeltaTime;
            }
            
            if (laserPool.Count > 0 && timeSinceLastAttack <= 0f)
            {
                animator.SetTrigger("attack");
                animator.SetBool("attackFinished", false);
                isAttacking = true;
            }
        }

        if (isAttacking)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            animator.SetBool("walking", false);
        }
    }

    public void Attack()
    {
        StartCoroutine(LaserAttack());
    }

    public void OnAttackComplete()
    {
        animator.ResetTrigger("attack");
        animator.SetBool("attackFinished", true);
        isAttacking = false;
        timeSinceLastAttack = attackTimer;
        rb.gravityScale = 1f;
    }

    IEnumerator LaserAttack()
    {
        rb.gravityScale = 0f;
        while (!(transform.position.y + .5f > player.transform.position.y &&
            transform.position.y - 1f < player.transform.position.y))
        {
            float step = verticalSpeed * Time.fixedDeltaTime;
            Vector3 targetPosition = new Vector3(transform.position.x,
                player.transform.position.y, 0f);
            if (transform.position.y < -5f)
            { 
                targetPosition.y = Mathf.Clamp(targetPosition.y, -33f, -26.75f);
            }
            else
            {
                targetPosition.y = Mathf.Clamp(targetPosition.y, -4f, 2.25f);
            }

            transform.position = Vector3.MoveTowards(transform.position,
                targetPosition, step);
            if ((targetPosition.x > player.transform.position.x && control.facingRight) ||
                (targetPosition.x < player.transform.position.x && !control.facingRight))
            {
                control.Flip();
            }

            yield return null;
        }

        RaycastHit2D hit = Physics2D.Raycast(globalShootPosition,
            control.facingRight ? Vector2.right : -Vector2.right, Mathf.Infinity, laserCollisionMask);
        if (hit) 
        {
            chargeLight.gameObject.SetActive(true);
            float intensity = chargeLight.intensity;
        
            for (int i = 0; i < framesBeforeLaser; i++)
            {
                float percentage = (float) i * (1f / framesBeforeLaser);
                chargeLight.intensity = Mathf.Lerp(0f, intensity, percentage);
                yield return null;
            }

            laserQueue = new Queue<Projectiles.Projectile>();

            for (float dx = 0f; dx < hit.distance; dx += (float) 1f/8f)
            {
                Projectiles.Projectile laserCopy = laserPool.Dequeue();
                laserCopy.instance.SetActive(true);

                laserQueue.Enqueue(laserCopy);
                
                int directionX = control.facingRight ? 1 : -1;

                laserCopy.instance.transform.position = transform.position +
                    new Vector3(dx * directionX, 0f, 0f);
            }

            for (int i = 0; i < laserDurationFrames; i++)
            {
                yield return null;
            }

            RecallLasers();
        }
        OnAttackComplete();
    }

    void RecallLasers()
    {
        int length = laserQueue.Count;
        for (int i = 0; i < length; i++)
        {
            Projectiles.Projectile laserCopy = laserQueue.Dequeue();
            laserCopy.instance.SetActive(false);
            laserPool.Enqueue(laserCopy);
        }
        chargeLight.gameObject.SetActive(false);
    }

    public override void Die()
    {
        if (laserQueue.Count != 0)
        {
            RecallLasers();
        }
        gameObject.SetActive(false);
        EnemySpawner.bishopPool.Enqueue(new Projectiles.Projectile(rb, gameObject));
    }
}
