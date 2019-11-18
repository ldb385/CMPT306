using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider spookBar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float vals = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().spookLevel;
        spookBar.value = vals;
    }
}
