using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointModifier : MonoBehaviour
{

    // Two variables allow for easier abstraction to editor on modifiers
    [SerializeField] private int pointAddition = 0;
    [SerializeField] private int pointSubtraction = 0;
    
    // when object is destroyed update the point system
    void OnDestroy()
    {
        if (Points.score + pointAddition - pointSubtraction <= 0)
        {
            Points.score = 0;
        }
        else
        {
            int cur = Points.score;
            Points.score = pointAddition - pointSubtraction + cur;   
        }
    }
}
