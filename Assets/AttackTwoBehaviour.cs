using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTwoBehaviour : StateMachineBehaviour
{
    public float timer;
    public float minTime;
    public float maxTime;
    public float projectileSpeed;
    private GameObject Player;
    public GameObject RazorBlade;
    public AudioClip razorSound;
    public float razorVolume = 1f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minTime, maxTime);
        Player = GameObject.Find("Player");
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
        Vector3 direction = Player.transform.position- animator.transform.position;
        GameObject RazB = Instantiate(RazorBlade, animator.transform.position, Quaternion.LookRotation(Vector3.forward, Player.transform.position - animator.transform.position));
        RazB.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;
        RazB.GetComponent<Rigidbody2D>().angularVelocity = -1000f;

        AudioSource.PlayClipAtPoint(razorSound, RazB.transform.position, razorVolume);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

}
