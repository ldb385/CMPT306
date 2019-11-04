﻿using System.Collections;
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
    public float attackRange;

    // bool for coroutine in ranged attack
    public bool canShoot = true;
    public float cooldownTime;

    // skeleton health
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
            // health -= GameObject.Find("Player").GetComponent<Player>().FireballDMG;

            if (health <= 0)
            {
                // play death sound/animation here

                Destroy(gameObject);

            }
        }
        if (col.gameObject.CompareTag("Balloon"))
        {
            
            health -= GameObject.Find("Player").GetComponent<Player>().BalloonDMG;

            if (health <= 0)
            {
                // play death sound/animation here

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
        // check if player is in range
        if ( Vector2.Distance(transform.position, target.GetComponent<Transform>().position) <= attackRange )
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

        
    }
}