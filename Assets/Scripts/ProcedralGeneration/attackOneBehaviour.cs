using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackOneBehaviour : StateMachineBehaviour
{
    public float timer;
    public float minTime;
    public float maxTime;

     private float attackRate = 1;
     private float attackTime = 0;

    // public float projectileSpeed;
    // private GameObject Player;
    // public GameObject[] SpeechBubbles;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Player = GameObject.Find("Player");
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
        if (Time.time > attackTime){
            attackTime = Time.time + attackRate;
            animator.GetComponent<BossData>().AttackOne();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

}
