using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Since you are starting actual map should set score to Zero
        Points.score = 0;
        GlobalControl.Instance.spookLevel = 0;
        GlobalControl.Instance.BalloonCount = 3;
    }
}
    
