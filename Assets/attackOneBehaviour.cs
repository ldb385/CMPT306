using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackOneBehaviour : StateMachineBehaviour
{
    public float timer;
    public float minTime;
    public float maxTime;
    public float projectileSpeed;
    private GameObject Player;
    public GameObject[] SpeechBubbles;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player = GameObject.Find("Player");
        timer = Random.Range(minTime, maxTime); 
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (timer<= 0){
           animator.SetTrigger("Idle");
       }
       else{
           timer -= Time.deltaTime;
       }

       // instantiate object and attack player here

        int rand = Random.Range(0,SpeechBubbles.Length);
        Vector3 direction = Player.transform.position - animator.transform.position; 
        GameObject speechB = Instantiate(SpeechBubbles[rand], animator.transform.position, Quaternion.LookRotation(Vector3.forward, Player.transform.position - animator.transform.position));
        speechB.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

}
