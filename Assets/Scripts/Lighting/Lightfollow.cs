using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightfollow : MonoBehaviour
{

    public GameObject player;        //Public variable to store a reference to the player game object

    private Vector3 offset;            //Private variable to store the offset distance between the player and camera

    private Quaternion StartPos;
    
    // Start is called before the first frame update
    void Start()
    {
        // sets the offset and starting position
        offset = transform.position - player.transform.position;
        StartPos = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {   
        //moves position of spotlight to player position
        follow();
    }

    void follow(){
        // transforms the positon to player
        transform.position = player.transform.position + offset;

        // rotates the light according to mouse position
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward)* StartPos;
    }

}