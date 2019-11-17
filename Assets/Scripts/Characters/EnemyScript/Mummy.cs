﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : MonoBehaviour
{
    public float health = 10f;
    public float speed = 2.3f;
    public int MummyDamage = 1;
    private Transform target;

    // create audio clips uncomment when we have audio selected
    // public AudioClip deathClip;
    public AudioClip groanClip;
    // public AudioClip damageClip;

    // coroutine variables
    public float cooldownTime = 3f;
    public bool canGroan = true;

    // Stop enemy from ending up on top of player
    private float stopDist = 0.65f;



    /**
    * simple following command that follows target based off vector position
    */
    void Chase()
    {
        // this will be used to chase the player
        transform.position = Vector2.MoveTowards(transform.position, target.position,
            speed * Time.deltaTime);

        // play mummy sound if not on cooldown
        if (canGroan)
        {
            StartCoroutine(Groan());
        }
    }

    public IEnumerator Groan()
    {
        // play laugh
        AudioSource.PlayClipAtPoint(groanClip, transform.position);

        // cooldown begins here
        canGroan = false;

        // wait for cooldownTime seconds
        yield return new WaitForSeconds(cooldownTime);

        // cooldown time ends
        canGroan = true;
    }
    private void movement()
    {
        if (Vector2.Distance(transform.position, target.position) > stopDist)
        {
            Chase();
        }
    }

    // detect if hit by projectile
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Projectile"))
        {
            health -= GameObject.Find("Player").GetComponent<Player>().FireballDMG;
        }
        else if(col.gameObject.CompareTag("Player")){
            col.gameObject.SendMessage("DamagePlayer", MummyDamage);
        }
    }

    // damages this GameObject(Mummy)
    public void ApplyDamage(float damage){
        health-=damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        // set the target as the player
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
         if (health <= 0)
            {
                // play death sound/animation here

                Destroy(gameObject);

            }
    }

    private void FixedUpdate()
    {
        // Perform Mummy movement
        movement();
    }
}
