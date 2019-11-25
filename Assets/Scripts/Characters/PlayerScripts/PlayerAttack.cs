using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //public AudioClip projectileClip;
    public GameObject playerProjectile;
    public GameObject WaterBaloon;
    public int ballonAmmo = 3;
    public float projectileSpeed = 1f;
    private float FireCoolDown = 0;
    private float WaterCoolDown = 0;
    private const float ShootInterval = 0.5f;
    //public AudioClip waterClip;
    public AudioClip projectileClip;
    public float attackVolume = 1.5f;

    private void Update()
    {
        FireCoolDown -= Time.deltaTime;
        WaterCoolDown -= Time.deltaTime;
    }

    void Start(){
      ballonAmmo = GlobalControl.Instance.BalloonCount;
    }

    public void Shoot()
    {
        // fire speed limiter
        if (FireCoolDown <= 0)
        {
            FindObjectOfType<Player>().animator.SetTrigger("Attack");
            // get positions
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = clickPosition - transform.position;

            // play projectile sound
            AudioSource.PlayClipAtPoint(projectileClip, transform.position, attackVolume);

            // create projectile and make it move
            GameObject projectile = Instantiate(playerProjectile, transform.position, Quaternion.LookRotation(Vector3.forward, clickPosition - transform.position));
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;

            // projectile can travel through player
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            FireCoolDown = ShootInterval;
        }
    }

    public void ThrowBalloon()
    {
        if (WaterCoolDown <= 0 && ballonAmmo > 0)
        {
            ballonAmmo--;
            // gets mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // gets balloon position
            Vector3 BalloonPosition = new Vector2(transform.position.x, transform.position.y);
            // gets distance between the two
            Vector3 direction = mousePos - BalloonPosition;
            direction.Normalize();

            // play throwing sound
            //AudioSource.PlayClipAtPoint(waterClip, transform.position);

            // create projectile and make it move
            GameObject watBalloon = Instantiate(WaterBaloon, BalloonPosition, Quaternion.identity);
            // calculates the distance and speed of balloon
            float distance = Vector3.Distance(watBalloon.transform.position, mousePos);
            float moveSpeed = Mathf.Clamp(distance * 4f, 50f, 250f);

            // move the balloon
            watBalloon.GetComponent<Rigidbody2D>().velocity = (direction * moveSpeed / 2);
            watBalloon.GetComponent<Rigidbody2D>().angularVelocity = -1000f;

            // ignore collition between player and balloon
            Physics2D.IgnoreCollision(watBalloon.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            WaterCoolDown = ShootInterval;
        }

    }
    public void SaveBalloons()
{
    GlobalControl.Instance.BalloonCount = ballonAmmo;
    }
}
