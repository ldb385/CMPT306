using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animator animator;

    // Enable the rigidBody attached to the door
    private void enableDoor()
    {
        animator.SetBool("DoorState", true);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        
    }

    // Disable the rigidBody attached to the door
    private void disableDoor()
    {
        animator.SetBool("DoorState", false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    // this will check whether enemies exist in the scene or not
    private bool enemiesExist()
    {
        if ( GameObject.FindGameObjectsWithTag("Enemies").Length  > 0 )
        {
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        // If enemies are within the scene all doors should be closed
        if ( enemiesExist() )
        {
            enableDoor();
        }
        else
        {
            disableDoor();
        }
    }
}
