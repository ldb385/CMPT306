using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // destroy projectile after 4 seconds if it hasn't hit anything
        Destroy(gameObject, 4.0f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemies"))
        {
            // play animation/sound?

            Destroy(gameObject);
        }
    }
}
