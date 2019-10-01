using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 3;
    public Transform target;

    void Start()
    {
        // set target position for projectile
        target = Input.mousePosition;
    }

    void Update()
    {
        // move projectile
        float increment = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.position, increment);
    }
}