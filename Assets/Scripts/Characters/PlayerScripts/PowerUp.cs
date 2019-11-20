using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Will Destroy the pickup only if the player goes over it
        if (other.CompareTag("Player"))
        {
            CleanUp();
        }
        
    }

    private void CleanUp()
    {
        Destroy(gameObject);
    }
}
