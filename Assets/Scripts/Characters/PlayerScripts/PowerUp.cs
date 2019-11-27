using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public AudioClip chompSound;
    public float volume = 1f;
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Will Destroy the pickup only if the player goes over it
        if (other.CompareTag("Player"))
        {
            // play chomp sound when the candy gets eaten
            AudioSource.PlayClipAtPoint(chompSound, transform.position, volume);
            CleanUp();
        }
        
    }

    private void CleanUp()
    {
        Destroy(gameObject);
    }
}
