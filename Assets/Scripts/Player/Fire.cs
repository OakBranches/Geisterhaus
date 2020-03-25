using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Fire : MonoBehaviour
{
    Animator animator;

    public Queue<Projectiles.Projectile> projectilePool =
        new Queue<Projectiles.Projectile>();

    public GameObject projectile;
    public int numProjectiles = 5;
    public float projectileSpeed = 1f;
    public float projectileTimer = 0.25f;
    float lastProjectileTimer = 0f;

    Light2D rechargeLight;
    float intensity;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rechargeLight = GetComponentInChildren<Light2D>();
        intensity = rechargeLight.intensity;
        rechargeLight.enabled = false;

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

        if (Input.GetButton("Fire1") && projectilePool.Count > 0 && lastProjectileTimer <= 0f)
        {
            Projectiles.Projectile projectile = projectilePool.Dequeue();
            projectile.instance.SetActive(true);
            projectile.instance.transform.position = transform.position;
            projectile.rb.velocity =
                new Vector3(projectileSpeed * transform.localScale.x, 0f, 0f);
            lastProjectileTimer = projectileTimer;
            animator.SetBool("shooting", true);
            StartCoroutine(RechargeLight());
        }
    }

    IEnumerator RechargeLight()
    {
        rechargeLight.enabled = true;
        rechargeLight.intensity = intensity;

        while (rechargeLight.intensity > 0)
        {
            float percentage = lastProjectileTimer / projectileTimer;
            rechargeLight.intensity = Mathf.Lerp(0f, intensity, percentage);
            yield return null;
        }

        rechargeLight.enabled = false;
        yield break;
    }

    void ResetAnimation()
    {
        animator.SetBool("shooting", false);
    }
}
