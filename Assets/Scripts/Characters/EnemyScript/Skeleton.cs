using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{

    // this will be the player object
    // Will be found based off tag in the start() function
    private GameObject target;

    // following, used specifically for lookAt function
    private Vector2 relativeTarget;
    private bool lookingRight;

    // create audio clips
    public AudioClip deathClip;
    public AudioClip projectileClip;
    // public AudioClip damageClip;

    // Ranged attack radius for skeleton attack
    public float attackRange = 4f;

    public GameObject enemyProjectile;

    // bool for coroutine in ranged attack
    public bool canShoot = true;
    public float cooldownTime = 3f;

    // skeleton health
    public float health = 10f;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // set target as player
        target = GameObject.FindWithTag("Player");

        // set orientation of enemy
        lookingRight = true;
        lookAt();

    }

    // detect if hit by projectile
    void OnCollisionEnter2D(Collision2D col)
    {
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
    }

    /**
     * this function will just make the skeleton flip based off the player location
     * basically just flips sprite based off player location
     */
    void lookAt()
    {
        if (relativeTarget.x > 0f)
        {
            // On right side
            if (lookingRight) // if not looking right, look other way
            {
                transform.Rotate(0f, 180f, 0f);
                lookingRight = true;
            }
        }
        else if (relativeTarget.x < 0f)
        {
            // On left side
            if (!lookingRight) // if looking right, look left
            {
                transform.Rotate(0f, 180f, 0f);
                lookingRight = false;
            }
        }
    }


    /**
     * This will check if the enemy is in range and do an ranged attack if so
     */


    bool inRange()
    {
        // check if player is in range
        if (Vector2.Distance(transform.position, target.GetComponent<Transform>().position) <= attackRange)
        {
            if (canShoot)
            {
                StartCoroutine(rangedAttack());
            }
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
        Vector2 playerPosition = target.transform.position;

        // get positions
        Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = playerPosition - enemyPosition;
        direction.Normalize();

        // create projectile and make it move
        GameObject projectile = Instantiate(enemyProjectile, enemyPosition, Quaternion.identity);

        projectile.GetComponent<Rigidbody2D>().velocity = direction * 6f;
        projectile.GetComponent<Rigidbody2D>().angularVelocity = -1000f;

        // projectile can travel through player
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        // play projectile sound
        AudioSource.PlayClipAtPoint(projectileClip, transform.position);

        // cooldown begins here
        canShoot = false;

        // wait for cooldownTime seconds
        yield return new WaitForSeconds(cooldownTime);

        // cooldown time endsy
        canShoot = true;
    }


    // Update is called once per frame
    void Update()
    {
        // check if player is in range for ranged attack
        inRange();

        // Can just run look at since checks are called in function
        lookAt();

        if (health <= 0)
        {
            // play death sound/animation here

            Destroy(gameObject);

        }
    }


    private void FixedUpdate()
    {
        if (!inRange())
        {
            animator.SetFloat("Xinput", (target.transform.position - transform.position).normalized.x);
            animator.SetFloat("Yinput", (target.transform.position - transform.position).normalized.y);
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 1 * Time.deltaTime);
        }
    }
}
