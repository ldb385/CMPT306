using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Mummy : MonoBehaviour
{
    public float health = 10f;
    public float speed;
    private Transform target;
    
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
            health--;

            if (health <= 0)
            {
                // play death sound/animation here
                
                Destroy(gameObject);

            }
        }
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

    }

    private void FixedUpdate()
    {
        // Perform Mummy movement
        movement();
    }
}
