
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemies : MonoBehaviour
{
    public Transform target;
    
    public float speed;
    public float rangeDist;
    public float chaseDist;
    
    
    void Chase()
    {
        // this will be used to chase the player
        transform.position = Vector2.MoveTowards(transform.position, target.position, 
            speed * Time.deltaTime);
        // may also apply damage?
    }
    
    void Ranged()
    {
        // nothing yet
    }
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }
    
    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(transform.position, target.position) >= chaseDist)
        {
            Chase();
        }
        if (Vector2.Distance(transform.position, target.position) <= rangeDist)
        {
            Ranged();
        }
    }
}
