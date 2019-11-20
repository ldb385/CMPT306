using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class will allow for comunication of score between scenes
public static class Points
{
    public static int score
    {
        get 
        {
            return score;
        }
        set 
        {
            Debug.Log("PointUpdatesd");
            score = value;
        }
    }
}
