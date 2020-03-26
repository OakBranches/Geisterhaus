using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class Priest : MonoBehaviour
{
    bool AttackMode;
    Animator animator;
    BoxCollider2D boxCollider;
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
    public void SetAttackMode(bool i){
        AttackMode=i;
    }
    public bool getAttackMode(){return AttackMode;}
    // Start is called before the first frame update
    void Start()
    {
        AttackMode=false;
        animator = GetComponent<Animator>();
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
        if(AttackMode){
        // A_Star();
        if (timeSinceLastAttack > 0)
        {
            timeSinceLastAttack -= Time.fixedDeltaTime;
        }
        
        if (projectilePool.Count > 0 && timeSinceLastAttack <= 0f)
        {
            animator.SetTrigger("attack");
            animator.SetBool("attackFinished", false);
            timeSinceLastAttack = attackTimer;
        }
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
