using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Set a spook meter
    public float spookLevel = 0;
    public float speed;

    // For shooting
    public int ballonAmmo = 0;
    public float BalloonDMG= 5.0f;
    public float FireballDMG = 1.0f; 

    // create audio clip
    public AudioClip deathClip;

    // footstep sounds
    public AudioSource _as;
    public AudioClip footstepClip;

    // create rigid body
    private Rigidbody2D rb;

    // create faceing bool
    private bool faceRight = true;

    // Create the movement velocity
    private Vector2 moveVel;

	// Create invincible state
	private bool invincible = false;

    // footstep sound variables
    private bool canWalk = true;
    public float footstepDelay;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVel = moveInput.normalized * speed;

        // check if the player is moving and play footset sounds
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            PlayFootstep();
        }

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
            FindObjectOfType<PlayerAttack>().Shoot();
 
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            if (ballonAmmo > 0){
                ballonAmmo -= 1;
                FindObjectOfType<PlayerAttack>().ThrowBalloon();
            }
        }
    

    }

    private void FixedUpdate()
    {
        rb.MovePosition( rb.position + moveVel * Time.fixedDeltaTime );

    }

    // check if the enemy physically touches the player (demonstrating melee damage for now?)
    void OnCollisionEnter2D(Collision2D col)
    {
        // check if the object the player collided with was an enemy or Pick up
        if (col.gameObject.CompareTag("Enemies") || col.gameObject.CompareTag("EnemyProjectile"))
        {
			if(!invincible){
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
    }

	// check if the player has picked up an item
	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.CompareTag("Soda")){
            StartCoroutine(SodaPickUp());
        }
        else if(c.gameObject.CompareTag("Cape")){
            StartCoroutine(CapePickUp());
        }
        else if(c.gameObject.CompareTag("Flashlight")){
            FlashlightPickUp();
        }
        else if(c.gameObject.CompareTag("Nerf")){
            StartCoroutine(NerfGunPickUp());
        }
        else if(c.gameObject.CompareTag("Teddy")){
            TeddyBearPickUp();
        }
        else if(c.gameObject.CompareTag("Candy")){
            CandyCornPickUp();
        }
        else if(c.gameObject.CompareTag("Balloon")){
            WaterBalloonPickUp();
        }
	}
    
    // flips the sprite on key press
    private void Flip(){
            faceRight = !faceRight;
            transform.Rotate(0f, 180f, 0f);

    }
    
    // SodaPickUp changes the speed to 2.0(?)
    public IEnumerator SodaPickUp(){
            speed = 2.0f;
            yield return new WaitForSecondsRealtime(5);
            speed = 1.0f;
    }
    
    // CapePickUp gives the player immunity from damage for 15(?) seconds
    public IEnumerator CapePickUp(){
            invincible = true;
            yield return new WaitForSecondsRealtime(15);
			invincible = false;
    }
    
    // FlashlightPickUp despooks player by 5(?)
    public void FlashlightPickUp(){
            spookLevel -=5.0f;
    }
    

    /***NEED TO FIX***/

    // NerfGunPickUp gives the player rapid fire for 20(?) seconds
    public IEnumerator NerfGunPickUp(){
            FindObjectOfType<PlayerAttack>().projectileSpeed = 25.0f;
            yield return new WaitForSecondsRealtime(20);
            FindObjectOfType<PlayerAttack>().projectileSpeed = 15.0f;
    }
    
    // TeddyBearPickUp despooks the player by 2(?)
    public void TeddyBearPickUp(){
            spookLevel -=2.0f;
    }
    
    // CandyCornPickUp despooks the player by 1(?)
    public void CandyCornPickUp(){
            spookLevel -=1.0f;
    }
    
    // WaterBallonPickUp adds a waterballon to the current amount of balloons
    public void WaterBalloonPickUp(){
            ballonAmmo +=1;
    }

    // Play footstep sound based on canWalk interval
    public void PlayFootstep()
    {
        if (canWalk)
        {
            StartCoroutine(PlayFootsteps());
        }
    }

    // Coroutine for previous function
    public IEnumerator PlayFootsteps()
    {   

        // play footstep sound
        _as.PlayOneShot(footstepClip);

        // cooldown starts
        canWalk = false;

        // play footstep sound every footstepDelay seconds
        yield return new WaitForSeconds(footstepDelay);

        canWalk = true;
    }

}
