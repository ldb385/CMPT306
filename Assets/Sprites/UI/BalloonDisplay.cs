using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonDisplay : MonoBehaviour
{
    public Text BalloonText;
    private GameObject player;
    private void Start() {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        BalloonText.text = player.GetComponent<PlayerAttack>().ballonAmmo.ToString();
    }
}
