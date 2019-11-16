using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;



public class Ghost : MonoBehaviour

{
    // Ghost speed, Note, this should be faster then average since the player will not
    // immidiately be targeted

    public float speed = 2.8f;
    public float health = 10f;
    public int GhostDamage = 1;

    // Stop enemy from ending up on top of player
    private float stopDist = 0.65f;

    // these are used for detection and movement of AI
    public float visibilityDistance = 4f;
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 1f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;

    // create audio clips
    //public AudioClip deathClip;
    public AudioClip chaseClip;
    // public AudioClip damageClip;

    // bool for coroutine in ranged attack
    public bool canLaugh = true;
    public float cooldownTime;

    private Transform target;



    void calcuateNewMovementVector()
    {
        //create a random direction vector with the magnitude of 1, later multiply it with the velocity of the enemy
        movementDirection = new Vector2(Random.Range(target.position.x - 0.3f, target.position.x + 0.3f),
            Random.Range(target.position.y, target.position.y)).normalized;
        movementPerSecond = movementDirection * 1.2f;
    }



    private bool canSeePlayer()
    {
        // check if the enemy is close enough to charge
        if (Vector2.Distance(transform.position, target.position) <= visibilityDistance)
        {
            return true;
        }

        // the player was not detected within either view
        return false;
    }

    /**
     * run at players position
     */

    private void Chase()
    {
        // this will be used to chase the player
        transform.position = Vector2.MoveTowards(transform.position, target.position,
            speed * Time.deltaTime);

        // play laughter sound if not on cooldown
        if (canLaugh)
        {
            StartCoroutine(Laugh());
        }
    }

    public IEnumerator Laugh()
    {
        // play laugh
        AudioSource.PlayClipAtPoint(chaseClip, transform.position);

        // cooldown begins here
        canLaugh = false;

        // wait for cooldownTime seconds
        yield return new WaitForSeconds(cooldownTime);

        // cooldown time ends
        canLaugh = true;
    }

    /**
     * Make enemy move freely
     */
    private void wander()
    {
        //if the changeTime was reached, calculate a new movement vector
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            calcuateNewMovementVector();
        }

        //move enemy: 
        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime),
            transform.position.y + (movementPerSecond.y * Time.deltaTime));
    }

    /**
     * Executes enemy movement
     */

    private void movement()
    {
        if (Vector2.Distance(transform.position, target.position) > stopDist)
        {
            if (canSeePlayer())
            {
                Chase();
            }
            else
            {
                wander();
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        // Set player as target
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Used for Enemy movement
        latestDirectionChangeTime = 0f;
        calcuateNewMovementVector();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement();
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            calcuateNewMovementVector();
        }
        // damage for ghost
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            health -= GameObject.Find("Player").GetComponent<Player>().FireballDMG;
        }
        else if(collision.gameObject.CompareTag("Player")){
            collision.gameObject.SendMessage("DamagePlayer", GhostDamage);
        }

    }
    private void Update()
    {
        if (health <= 0)
        {
            // play death sound/animation here

            Destroy(gameObject);

        }
    }
}