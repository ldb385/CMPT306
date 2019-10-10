﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Set a spook meter
    public float spookLevel = 0;
    public float speed;

    // For shooting
    public GameObject playerProjectile;
    public float projectileSpeed = 15.0f;

    // create audio clip
    public AudioClip deathClip;
    public AudioClip projectileClip;

    // create rigid body
    private Rigidbody2D rb;

    // create faceing bool
    private bool faceRight;

    // Create the movement velocity
    private Vector2 moveVel;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2( Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVel = moveInput.normalized * speed;
        if (Input.GetKeyDown(KeyCode.D)){
            if(faceRight==true){
                Flip();
            }
        }
        else if(Input.GetKeyDown(KeyCode.A)){
            if(faceRight==false){
                Flip();
            }
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition( rb.position + moveVel * Time.fixedDeltaTime );
    }

    // check if the enemy physically touches the player (demonstrating melee damage for now?)
    void OnCollisionEnter2D(Collision2D col)
    {
        // check if the object the player collided with was an enemy
        if (col.gameObject.CompareTag("Enemies"))
        {
            // players spook level goes up
            spookLevel++;

            // if spook meter is full, player is "killed"
            if(spookLevel >= 10)
            {
                // play death sound
                AudioSource.PlayClipAtPoint(deathClip, transform.position);
                // mark object destroyed in the next frame
                Destroy(gameObject);

            }
        }
    }

    //void DestroyProjectile(Collision2D obstacle, GameObject projectile)
    //{
    //    if (obstacle.gameObject.CompareTag("Obstacle"))
    //    {
    //        // play animation/sound here?
    //        Destroy(projectile);
    //    }

    //    // destroy projectile after 4 seconds if it hasn't hit anything
    //    Destroy(projectile, 4.0f);
    //}

    void Shoot()
    {
        // get positions
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = clickPosition - playerPosition;
        direction.Normalize();

        // play projectile sound
        AudioSource.PlayClipAtPoint(projectileClip, transform.position);

        // create projectile and make it move
        GameObject projectile = Instantiate(playerProjectile, playerPosition, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

        // destroy projectile after 4 seconds if it hasn't hit anything
        Destroy(projectile, 4.0f);

        // remove above line and use DestroyProjectile once obstacles implemented
        //DestroyProjectile(obstacle, projectile);
    }

    // flips the sprite on key press
    private void Flip(){
            faceRight = !faceRight;
            transform.Rotate(0f, 180f, 0f);

    }
}
