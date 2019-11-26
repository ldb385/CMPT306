using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animator animator;

    public AudioClip doorClose;
    // public AudioClip doorOpen;
    public bool closeHasPlayed;
    public bool openHasPlayed;


    // Enable the rigidBody attached to the door
    private void enableDoor()
    {
        animator.SetBool("DoorState", true);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        if (!closeHasPlayed)
        {
            AudioSource.PlayClipAtPoint(doorClose, transform.position);
            closeHasPlayed = true;
        }
    }

    // Disable the rigidBody attached to the door
    private void disableDoor()
    {
        animator.SetBool("DoorState", false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if (!openHasPlayed)
        {
            // AudioSource.PlayClipAtPoint(doorOpen, transform.position, 0.05f);
            openHasPlayed = true;
        }
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
            openHasPlayed = false;
            enableDoor();
        }
        else
        {
            closeHasPlayed = false;
            disableDoor();
        }
    }
}
