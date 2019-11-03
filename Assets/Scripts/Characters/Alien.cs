using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float speed;
    private Transform target;
    public float health = 10f;

   // create audio clips *** UNCOMMENT WHEN AUDIO CLIPS ARE CHOSEN ***
   // public AudioClip deathClip;
   public AudioClip projectileClip;
   // public AudioClip damageClip;

    // Ranged attack radius for alien attack
    public float attackRange;

    // bool for coroutine in ranged attack
    public bool canShoot = true;
    public float cooldownTime;

    // Stop enemy from ending up on top of player
    private float stopDist = 0.65f;

    // detect if hit by projectile
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Projectile"))
        {
            health--;
            // AudioSource.PlayClipAtPoint(damageClip, transform.position);
            if (health <= 0)
            {
                // play death sound/animation here
                // AudioSource.PlayClipAtPoint(deathClip, transform.position);
                Destroy(gameObject);

            }
        }
    }


    /**
     * This will check if the enemy is in range and do an ranged attack if so
     */
    void inRange()
    {
        // check if player is in range
        if (Vector2.Distance(transform.position, target.GetComponent<Transform>().position) <= attackRange)
        {
            // check if on cooldown
            if (canShoot)
            {
                StartCoroutine(rangedAttack());
            }
        }
    }

    /**
     * Perform the ranged attack
     */
    public IEnumerator rangedAttack()
    {
        // projectile sprite goes here

        // play projectile sound *** UNCOMMENT WHEN AUDIO CLIP IS CHOSEN *** 
        AudioSource.PlayClipAtPoint(projectileClip, transform.position);

        // cooldown begins here
        canShoot = false;

        // wait for cooldownTime seconds
        yield return new WaitForSeconds(cooldownTime);

        // cooldown time endsy
        canShoot = true;

    
    // Start is called before the first frame update
    void Start()
    {
        // set the target as the player
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // check if player is in range
        inRange();
        }
    }
}
