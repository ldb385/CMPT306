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
    public GameObject playerProjectile;
    public float projectileSpeed = 1f;
    public float ballonAmmo = 0.0f;
    
    // create audio clip
    public AudioClip deathClip;
    public AudioClip projectileClip;

    // create rigid body
    private Rigidbody2D rb;

    // create faceing bool
    private bool faceRight = true;

    // Create the movement velocity
    private Vector2 moveVel;

	// Create invincible state
	private bool invincible = false;
    
    public Camera cam;
    
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
        // check if the object the player collided with was an enemy or Pick up
        if (col.gameObject.CompareTag("Enemies"))
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
        //Debug.Log("Working");
        //if (col.gameObject.name == "circle(Clone)")
        //{
        //    Destroy(col.gameObject);
        //}
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

    void Shoot()
    {
        // get positions
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = clickPosition - playerPosition;
        direction.Normalize();

        // play projectile sound
        AudioSource.PlayClipAtPoint(projectileClip, transform.position);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // create projectile and make it move
        GameObject projectile = Instantiate(playerProjectile, playerPosition, Quaternion.LookRotation(Vector3.forward, mousePos - transform.position));
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

        // projectile can travel through player
		Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
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
    
    // NerfGunPickUp gives the player rapid fire for 20(?) seconds
    public IEnumerator NerfGunPickUp(){
            projectileSpeed = 25.0f;
            yield return new WaitForSecondsRealtime(20);
            projectileSpeed = 15.0f;
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
            ballonAmmo +=1.0f;
    }

}
