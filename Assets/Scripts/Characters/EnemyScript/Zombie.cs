
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

public class Zombie : MonoBehaviour
{
    // Target will always be player
    private GameObject target;
    // private float zombieAttackInterval = 0.5f;
    private float ZombieCoolDown = 0;
    private float attackRange = 0.3f;
    public int ZombieDamage = 1;

    // speed at which to follow player
    public float chaseSpeed = 2.2f;
    // speed at which to Charge at player
    public float chargeSpeed = 3.5f;
    // distance that charge will be called at
    public float chargeDist = 4f;

    // Stop enemy from ending up on top of player
    private float stopDist = 0.65f;

    private float _chargeFatigue;
    private bool _canCharge;

    // the max frames for zombie to be fatigued
    public float maxFatigue = 5f;

    // array to hold zombie sounds
    public AudioSource _as;
    public AudioClip[] audioClipArray;
    // public AudioClip deathClip;
    // public AudioClip damageClip;

    public float health = 10f;

    public Animator anim;
    
    public void ApplyDamage(float damage)
    {
        health -= damage;
    }
    /**
     * simple following command that follows target based off vector position
     */
    void Chase()
    {
        // *** Tint zombie when its not charging ( CAN BE TAKEN OUT ) ***
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        // this will be used to chase the player
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position,
            chaseSpeed * Time.deltaTime);
        if (_chargeFatigue > 0)
        {
            _chargeFatigue -= 1;
        }
    }

    /**
     * This will be used to increase speed when the zombie gets close enough to the player
     * after a certain amount of frames the zombie will become tired and slow its speed
     */
    void Charge()
    {
        // this function will increase speed slightly when it gets closer to the player
        if ( _canCharge )
        {
            Debug.Log("CHARGING");
            // *** Tint zombie when its charging ( CAN BE TAKEN OUT ) ***
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.9245283f, 0.7893378f, 0.7893378f, 1);

            // Charge after player
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position,
                chargeSpeed * Time.deltaTime);
            // get tired from charging
            _chargeFatigue += 1;
            if (_chargeFatigue >= maxFatigue)
            {
                // too tired can no longer charge till strength is regained
                _canCharge = false;
            }
        }
        else
        {
            // zombie is too tired to charge more and will just chase instead
            Chase();
            if (_chargeFatigue <= 0)
            {
                _canCharge = true;
            }
        }
    }

    /**
     * this function will be used to perform an attack from the
     * zombie when the player is close enough
     */
    public void zombieAttack()
    {
        Debug.Log("attack");
        // just a place holder for now
        target.gameObject.SendMessage("DamagePlayer", ZombieDamage);
    }

    void Start()
    {
        ZombieCoolDown -= Time.deltaTime;
        // initialize player as target
        target = GameObject.FindGameObjectWithTag("Player");

        // Setting up for charge functions
        _chargeFatigue = 0;
        _canCharge = true;

        // play zombie sound every 5 seconds
        InvokeRepeating("PlaySound", 0.001f, 5f);
    }

    void PlaySound()
    {
        _as.clip = audioClipArray[Random.Range(0, audioClipArray.Length -1)];
        _as.PlayOneShot(_as.clip);
    }

    /**
     * this is the runner of the Zombie's movement
     */
    private void movement()
    {
        if (Vector2.Distance(transform.position, target.transform.position) > stopDist)
        {
            // check if the enemy is close enough to charge
            if (Vector2.Distance(transform.position, target.transform.position) <= chargeDist)
            {
                Charge();
            }
            else
            {
                // if not close enough to charge simply chase
                Chase();
            }
        }
        anim.SetFloat("XInput", (target.transform.position - transform.position).normalized.x);
        anim.SetFloat("YInput", (target.transform.position - transform.position).normalized.y);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // check if player is close enough to attack the player
        if (Vector2.Distance(transform.position, target.transform.position) <= 1)
        {
                zombieAttack();
        }
        else{
            movement();
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
