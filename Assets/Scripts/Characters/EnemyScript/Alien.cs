using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float speed = 2.9f;
    private GameObject target;
    public float health = 10f;

    // create audio clips *** UNCOMMENT WHEN AUDIO CLIPS ARE CHOSEN ***
    // public AudioClip deathClip;
    public AudioClip projectileClip;
    // public AudioClip damageClip;

    private Rigidbody2D AlienRigidBody;

    public GameObject AlienProjectile;

    // Ranged attack radius for alien attack
    public float attackRange = 5f;

    // bool for coroutine in ranged attack
    private bool canShoot = true;
    public float cooldownTime;
    Vector3 origin;
    private Vector2 movementDirection;
    public Animator animator;

    // Stop enemy from ending up on top of player
    // private float stopDist = 0.65f;

    // detect if hit by projectile
    void OnCollisionEnter2D(Collision2D col)
    {

    }

    /**
     * This will check if the enemy is in range and do an ranged attack if so
     */
    bool inRange()
    {
        // check if player is in range
        if (Vector2.Distance(transform.position, target.GetComponent<Transform>().position) <= attackRange)
        {
            return true;
        }
        return false;
    }

    /**
     * Perform the ranged attack
     */
    public IEnumerator rangedAttack()
    {
        // projectile sprite goes here
        Vector3 playerPosition = target.transform.position;
        Vector3 direction = playerPosition - transform.position;
        direction.Normalize();

        // spawn the lazr
        GameObject projectile = Instantiate(AlienProjectile, transform.position, Quaternion.LookRotation(Vector3.forward, playerPosition - transform.position));
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 50f;
        // ignore collition with itself
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        // play projectile sound *** UNCOMMENT WHEN AUDIO CLIP IS CHOSEN *** 
        AudioSource.PlayClipAtPoint(projectileClip, transform.position);

        // cooldown begins here
        canShoot = false;

        // wait for cooldownTime seconds
        yield return new WaitForSeconds(cooldownTime);

        // cooldown time endsy
        canShoot = true;

    }
    public void ApplyDamage(float damage)
    {
        health -= damage;
    }


    // Start is called before the first frame update
    void Start()
    {
        // set the target as the player
        target = GameObject.FindWithTag("Player");
        origin = transform.position;
        AlienRigidBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // check if player is in range


        // check if on cooldown
        if (canShoot)
        {

            StartCoroutine(rangedAttack());
        }

        if (health <= 0)
        {
            // play death sound/animation here
            Destroy(gameObject);

        }

    }

    private void FixedUpdate() {
        if(!inRange()){
            animator.SetFloat("Xinput", (target.transform.position - transform.position).normalized.x);
            animator.SetFloat("Yinput", (target.transform.position - transform.position).normalized.y);
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

}