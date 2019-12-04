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
    public int BalloonDMG= 5;
    public float FireballDMG = 1.0f;

    // create audio clip
    public AudioClip deathClip;
    public AudioClip damageClip;

    public float deathVolume = 1f;

    // footstep sounds
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

    // track if the player is alive
    public bool isAlive;

    public Animator animator;

	private float maxSpeed = 6.4f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isAlive = true;
        spookLevel = GlobalControl.Instance.spookLevel;
    }

    // Update is called once per frame
    void Update()
    {
        // movement animation
        animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVel = moveInput.normalized * speed;

        // check if the player is moving and play footset sounds
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            PlayFootstep();
        }

        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<PlayerAttack>().Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Space)){
                FindObjectOfType<PlayerAttack>().ThrowBalloon();
        }


    }

    private void FixedUpdate()
    {
        rb.MovePosition( rb.position + moveVel * Time.fixedDeltaTime );

    }


// New Damage system ( USE send Message IE. GameObject.SendMessage("DamagePlayer", damage_amount))
    public void DamagePlayer(int Damage){
       		if(!invincible){
                // play damage sound
                AudioSource.PlayClipAtPoint(damageClip, transform.position, 0.5f);

				// players spook level goes up
				spookLevel += Damage;
				// player getAway activates
				StartCoroutine(getAway());
				// if spook meter is full, player is "killed"
				if(spookLevel >= 10)
				{
					// play death sound
					AudioSource.PlayClipAtPoint(deathClip, transform.position, deathVolume);

                    // mark object destroyed in the next frame
                    isAlive = false;
                    // Destroy(gameObject);
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


    // SodaPickUp changes the speed to 2.0(?)
    public IEnumerator SodaPickUp(){
            if( speed < maxSpeed)
			{
				speed *= 2;
				yield return new WaitForSecondsRealtime(5);
				speed /= 2;
			}
			
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
             this.GetComponent<PlayerAttack>().ballonAmmo += 3;
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
        AudioSource.PlayClipAtPoint(footstepClip, transform.position);

        // cooldown starts
        canWalk = false;

        // play footstep sound every footstepDelay seconds
        yield return new WaitForSeconds(footstepDelay);

        canWalk = true;
    }

	// Coroutine for getAway function
	public IEnumerator getAway(){
		invincible = true;
		if( speed < maxSpeed)
		{
			StartCoroutine(SodaPickUp());
		}
        yield return new WaitForSecondsRealtime(1);
		invincible = false;
	}

	public void theOuchies()
	{
		StartCoroutine(equalizer());
	}

	public IEnumerator equalizer()
	{
		invincible = true;
        yield return new WaitForSecondsRealtime(1);
		invincible = false;
	}
  
  public void SavePlayer()
	{
    GlobalControl.Instance.spookLevel = spookLevel;
    }

}
