
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    // Target will always be player
    private Transform target;
    
    // speed at which to follow player
    public float chaseSpeed;
    // speed at which to Charge at player
    public float chargeSpeed;
    // distance that charge will be called at
    public float chargeDist;
    
    // Stop enemy from ending up on top of player
    private float stopDist = 0.65f;


    private int _chargeFatigue;
    private bool _canCharge;

    // the max frames for zombie to be fatigued
    public int maxFatigue;

    // array to hold zombie sounds
    public AudioSource _as;
    public AudioClip[] audioClipArray;


    /**
     * simple following command that follows target based off vector position
     */
    void Chase()
    {
        // this will be used to chase the player
        transform.position = Vector2.MoveTowards(transform.position, target.position, 
            chaseSpeed * Time.deltaTime);
    }
    
    /**
     * This will obe used to increase speed when the zombie gets close enough to the player
     * after a certain amount of frames the zombie will become tired and slow its speed
     */
    void Charge()
    {
        // this function will increase speed slightly when it gets closer to the player
        if ( (_chargeFatigue <= maxFatigue ) && _canCharge )
        {
            // *** Tint zombie when its charging ( CAN BE TAKEN OUT ) ***
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.9245283f, 0.7893378f,0.7893378f, 1);
            
            // Charge after player
            transform.position = Vector2.MoveTowards(transform.position, target.position, 
                chargeSpeed * Time.deltaTime);
            // get tired from charging
            _chargeFatigue++;
            if (_chargeFatigue == maxFatigue )
            {
                // too tired can no longer charge till strength is regained
                _canCharge = false;

            }
        } 
        else
        {
            // *** Tint zombie when its not charging ( CAN BE TAKEN OUT ) ***
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            
            // zombie is too tired to charge more and will just chase instead
            Chase();
            if (_chargeFatigue <= 0)
            {
                _canCharge = true;
            }
            // zombie regains strength
            _chargeFatigue--;

        }
    }

    /**
     * this function will be used to perform an attack from the
     * zombie when the player is close enough
     */
    void zombieAttack()
    {
        // just a place holder for now
    }
    
    void Start()
    {
        // initialize player as target
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Setting up for charge functions
        _chargeFatigue = 0;
        _canCharge = true;

        // play


        InvokeRepeating("PlaySound", 0.001f, 5f);

    }

    void PlaySound()
    {
        _as.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        _as.PlayOneShot(_as.clip);
        Debug.Log("zombie sound");
    }

    /**
     * this is the runner of the Zombie's movement
     */
    private void movement()
    {
        if (Vector2.Distance(transform.position, target.position) > stopDist)
        {
            // check if the enemy is close enough to charge
            if (Vector2.Distance(transform.position, target.position) <= chargeDist)
            {
                Charge();
            }
            else
            {
                // if not close enough to charge simply chase
                Chase();
            }
        }
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        movement();
        
        // check if player is close enough to attack the player
        if (Vector2.Distance(transform.position, target.position) <= 1)
        {
            zombieAttack();
        }
    }
}
