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

    // Ranged attack radius for skeleton attack
    public float attackRange;

    public float health = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // set target as player
        target = GameObject.FindWithTag( "Player" );
        
        // set orientation of enemy
        lookingRight = true;
        lookAt();

    }

    // detect if hit by projectile
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Projectile"))
        {
            health--;

            if (health <= 0)
            {
                // play death sound and destroy objectx
                AudioSource.PlayClipAtPoint(deathClip, transform.position);
                Destroy(gameObject);

            }
        }
    }

    /**
     * this function will just make the skeleton flip based off the player location
     * basically just flips sprite based off player location
     */
    void lookAt()
    {
        // Check where player is located and turn towards
        relativeTarget = transform.InverseTransformPoint( target.GetComponent<Transform>().position );
        if( relativeTarget.x > 0f )
        {
            // On right side
            if ( lookingRight ) // if not looking right, look other way
            {
                transform.Rotate(0f, 180f, 0f);
                lookingRight = true;
            }
        }
        else if( relativeTarget.x < 0f )
        {
            // On left side
            if ( !lookingRight ) // if looking right, look left
            {
                transform.Rotate(0f, 180f, 0f);
                lookingRight = false;
            }
        }
    }
    
    
    /**
     * This will check if the enemy is in range and do an ranged attack if so
     */
    void inRange()
    {
        if ( Vector2.Distance(transform.position, target.GetComponent<Transform>().position) <= attackRange )
        {
            rangedAttack();
        }
    }
    
    /**
     * Perform the ranged attack
     */
    void rangedAttack()
    {

        // play projectile sound
        AudioSource.PlayClipAtPoint(projectileClip, transform.position);
    }


    public float attackInterval;
    private float period = 0.0f;

    // Update is called once per frame
    void Update()
    {
        // rangedAttack(
        if(period > attackInterval)
        {
            rangedAttack();
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;


        // Can just run look at since checks are called in function
        lookAt();

        
    }
}
