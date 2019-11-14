using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonFire : MonoBehaviour
{
    private int HeightState = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // simulates the height change 
        switch(HeightState){
            default:
            case 0:
                transform.localScale += Vector3.one * 7f * Time.deltaTime;
                if(transform.localScale.x >= 2.5f) HeightState = 1;
                break;
            case 1:
                transform.localScale -= Vector3.one * 7f * Time.deltaTime;
                if(transform.localScale.x <= 1f) HeightState = 2;
                break;
            case 2:
                break;
        }
    }
}
