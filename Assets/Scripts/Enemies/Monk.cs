using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NPControl))]
public class Monk : Enemy
{
    Animator animator;
    BoxCollider2D boxCollider;
    Rigidbody2D rb;
    NPControl control;

    public Vector2 shieldPosition = Vector2.zero;
    Vector2 globalShieldPosition;
	public LifeManager lifeManager;
    public GameObject shield;
    public static Queue<Projectiles.Projectile> shieldPool;
    public float attackTimer = 2.5f;
    float timeSinceLastAttack = 0f;
    public int maxShields = 100;
    bool isAttacking = false;
    public float verticalSpeed = 5f;
    bool canAttack = true;

    public override void AttackMove(float speed, Vector3 toWhere)
    {
        if (!isAttacking)
        {
            float step = Time.deltaTime * speed;
            float directionX;

            if (toWhere == Vector3.zero)
            {
                toWhere.x = player.transform.position.x;
            }

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

            Vector3 targetPosition = new Vector3(toWhere.x, transform.position.y, 0f);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            rb.velocity = Vector3.zero;

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

        if (shieldPool == null)
        {
            shieldPool = new Queue<Projectiles.Projectile>();
        }
        
        for (int i = 0; i < maxShields; i++)
        {
            GameObject shieldObject = Instantiate(shield);
            Projectiles.Projectile shieldCopy = 
                new Projectiles.Projectile(shieldObject.GetComponent<Rigidbody2D>(),
                shieldObject);
            shieldPool.Enqueue(shieldCopy);
            shieldCopy.instance.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        globalShieldPosition = shieldPosition +
            new Vector2(transform.position.x, transform.position.y);
        if (attackMode)
        {
            if (timeSinceLastAttack > 0)
            {
                timeSinceLastAttack -= Time.fixedDeltaTime;
            }
            
            if (shieldPool.Count > 0 && timeSinceLastAttack <= 0f && canAttack)
            {
                animator.SetTrigger("attack");
                animator.SetBool("attackFinished", false);
                isAttacking = true;
                canAttack = false;
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
        StartCoroutine(ShieldAttack());
    }

    public void OnAttackComplete()
    {
        animator.ResetTrigger("attack");
        animator.SetBool("attackFinished", true);
        isAttacking = false;
        timeSinceLastAttack = attackTimer;
        canAttack = true;
    }

    IEnumerator ShieldAttack()
    {
        if (shieldPool.Count > 0)
        {
            Projectiles.Projectile shieldCopy = shieldPool.Dequeue();
            shieldCopy.instance.SetActive(true);
            shieldCopy.instance.transform.position = globalShieldPosition;

            Vector3 oldPlayerPosition = player.transform.position;

            while (!(shieldCopy.instance.transform.position.y + 1f > oldPlayerPosition.y &&
                shieldCopy.instance.transform.position.y - 1f < oldPlayerPosition.y))
            {
                float step = verticalSpeed * Time.fixedDeltaTime;
                Vector3 targetPosition = new Vector3(shieldCopy.instance.transform.position.x,
                    oldPlayerPosition.y, 0f);
                if (shieldCopy.instance.transform.position.y < -5f)
                { 
                    targetPosition.y = Mathf.Clamp(targetPosition.y, -33f, -26.75f);
                }
                else
                {
                    targetPosition.y = Mathf.Clamp(targetPosition.y, -4f, 2.25f);
                }

                shieldCopy.instance.transform.position = Vector3.MoveTowards(shieldCopy.instance
                    .transform.position, targetPosition, step);

                yield return null;
            }
        }

        yield return null;

        OnAttackComplete();
    }

    public override void Die()
    {
        gameObject.SetActive(false);
        EnemySpawner.monkPool.Enqueue(new Projectiles.Projectile(rb, gameObject));
    }
}
