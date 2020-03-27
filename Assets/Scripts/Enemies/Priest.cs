using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NPControl))]
public class Priest : Enemy
{
    Animator animator;
    BoxCollider2D boxCollider;
    Rigidbody2D rb;
    NPControl control;

    Vector3 shootPosition;
	public LifeManager lifeManager;
    public GameObject projectile;
    public static Queue<Projectiles.Projectile> projectilePool =
        new Queue<Projectiles.Projectile>();
    public float attackTimer = 2.5f;
    float timeSinceLastAttack = 0f;
    public int projectilesPerAttack = 8;
    public int maxAttacks = 15;
    public int framesBetweenShots = 4;
    public float projectileSpeed = 5f;
    bool isAttacking = false;

    public override void AttackMove(float speed)
    {
        if (!isAttacking)
        {
            float step = Time.deltaTime * speed;
            float toWhere = player.transform.position.x - transform.position.x;
            Vector3 velocity = new Vector3(speed, 0f, 0f);
            if (toWhere < -0.05)
            {
                velocity.x *= -1f;
            }

            if (-0.05f < toWhere && toWhere < 0.05f)
            {
                velocity.x = 0f;
            }

            if ((velocity.x < 0f && control.facingRight) ||
                (velocity.x > 0f && !control.facingRight))
            {
                control.Flip();
            }

            rb.velocity = velocity;

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
        
        for (int i = 0; i < projectilesPerAttack * maxAttacks; i++)
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
        if (attackMode)
        {
            if (timeSinceLastAttack > 0)
            {
                timeSinceLastAttack -= Time.fixedDeltaTime;
            }
            
            if (projectilePool.Count > 0 && timeSinceLastAttack <= 0f)
            {
                animator.SetTrigger("attack");
                animator.SetBool("attackFinished", false);
                timeSinceLastAttack = attackTimer;
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
        StartCoroutine(AttackSpiral());
    }

    public void OnAttackComplete()
    {
        animator.ResetTrigger("attack");
        animator.SetBool("attackFinished", true);
        isAttacking = false;
    }

    IEnumerator AttackSpiral()
    {
        float angle = 0f;
        for (int i = 0; i < projectilesPerAttack; i++)
        {
            Bounds bounds = boxCollider.bounds;
            shootPosition = new Vector3(transform.position.x,
                transform.position.y - bounds.extents.y / 2, 0f);

            angle += (2 * Mathf.PI) / projectilesPerAttack;

            Projectiles.Projectile projectileCopy = projectilePool.Dequeue();
            projectileCopy.instance.SetActive(true);
            
            projectileCopy.instance.transform.position = shootPosition;

            projectileCopy.rb.velocity = new Vector3(projectileSpeed * Mathf.Cos(angle),
                projectileSpeed * Mathf.Sin(angle), 0f);
            for (int j = 0; j < framesBetweenShots; j++)
            {
                yield return null;
            }
        }
    }
}
