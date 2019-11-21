using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class will allow for comunication of score between scenes
public static class Points
{
    private static int _score;
    public static int score {
        get 
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }
}
