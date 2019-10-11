using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Ghost speed, Note, this should be faster then average since the player will be able
    // to dodge attacks
    public float speed;
    
    
    /**
     * More complex following command that uses ray-casting to find enemy then follows based off
     * returned position, this allows for player to "sidestep" ghost in order to dodge attacks
     */
    void Chase()
    {
        // first need to send out a ray
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
