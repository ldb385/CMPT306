using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
      if(col.gameObject.CompareTag("Player")){
        GameObject.Find("Player").GetComponent<Player>().SavePlayer();
        GameObject.Find("Player").GetComponent<PlayerAttack>().SaveBalloons();
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex +1);
      }
  }
}
