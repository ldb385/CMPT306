using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextFloor_1 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
      if(col.gameObject.tag == "Player"){
        GameObject.Find("Player").GetComponent<Player>().SavePlayer();
        GameObject.Find("Player").GetComponent<PlayerAttack>().SaveBalloons();
        SceneManager.LoadScene("LevelTwo");
      }
  }
}
