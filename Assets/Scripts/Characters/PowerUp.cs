using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float multiplier = 1.5f;
    public float duration = 10f;
    public GameObject pickupEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
        else
        {
            Debug.Log("Power up can't be picked up by enemies.");
        }
    }

    IEnumerator Pickup(Collider player)
    {
        
        //Spawn some kind of effect
        Instantiate(pickupEffect, transform.position, transform.rotation);

        //Apply effect to player
        Player wiz = player.GetComponent<Player>();
        wiz.speed *= multiplier;

        //Timer
        //Makes the game object disappear
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(duration);
        //Returns to original value
        wiz.speed /= multiplier;

        //Clean Up and remove from scene
        Destroy(gameObject);

    }
}
