using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public AudioClip chompSound;
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Will Destroy the pickup only if the player goes over it
        if (other.CompareTag("Player"))
        {
            // play chomp sound when the candy gets eaten
            AudioSource.PlayClipAtPoint(chompSound, transform.position, 100f);
            CleanUp();
        }
        
    }

    private void CleanUp()
    {
        Destroy(gameObject);
    }
}
