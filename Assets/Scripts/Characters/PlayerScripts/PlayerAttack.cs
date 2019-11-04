using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AudioClip projectileClip;
    public GameObject playerProjectile;
    public GameObject WaterBaloon;
    public float projectileSpeed = 1f;
    private float CoolDown = 0;
    private const float ShootInterval = 0.5f;

    private void Update()
    {
        CoolDown -= Time.deltaTime;
    }

    public void Shoot()
    {
        // fire speed limiter
        if (CoolDown <= 0)
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
            CoolDown = ShootInterval;
        }
    }

    public void ThrowBalloon()
    {
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = clickPosition - playerPosition;
        direction.Normalize();

        // create projectile and make it move
        GameObject watBalloon = Instantiate(WaterBaloon, playerPosition, Quaternion.identity);
        watBalloon.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed / 2;

        // destroy projectile after 4 seconds if it hasn't hit anything
        Destroy(watBalloon, 1.0f);
    }
}
