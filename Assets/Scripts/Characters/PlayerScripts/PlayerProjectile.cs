using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // destroy projectile after 3 seconds if it hasn't hit anything
        Destroy(gameObject, 3.0f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // add fancy effects here

        if (collision.gameObject.tag != "Player")
        {
			Destroy(gameObject);
        }
        else
		{
			//Physics.IgnoreCollision(collision.GetComponent<collider>, collider);
			//Physics2D.IgnoreCollision(collision.GetComponent<BoxCollider2D>(), gameObject.GetComponent<CircleCollider2D>());
		}
    }
}
