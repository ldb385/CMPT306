using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Set a spook meter
    public float spookLevel;
    public float speed;
    
    // create rigid body
    private Rigidbody2D rb;
    // Create the movement velocity
    private Vector2 moveVel;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2( Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVel = moveInput.normalized * speed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition( rb.position + moveVel * Time.fixedDeltaTime );
    }
}
