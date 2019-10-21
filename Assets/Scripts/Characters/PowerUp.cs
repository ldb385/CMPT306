using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    
    public float oldSpeed = 3.0f;
    public GameObject player;
    //private float timer = 0.0f;
    
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
