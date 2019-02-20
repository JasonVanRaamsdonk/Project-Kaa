﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed; // speed variable used for movement
    public float fallMultiplier = 2.50f; // fall multiplier for high jumps
    public float lowMultiplier = 2.00f; // fall multiplier for low jumps
    public float jumpForce = 8.00f; // velocity when jumping 
    public LayerMask Ground; // used for setting up a raycast, put all tiles into a layer called Ground
    public float rayDistance = 1f; // used for setting ray length which will determine the distance from when player can jump
    public int extraJumpsValue; // how many jumps the player can have before grounding 
    public Animator animator;

    private int extraJumps;  
    private bool facingRight = true; // flag if the player is facing right
    private Rigidbody2D rb2d; // shorthand for <rigidbody2d>
    private float movement = 0f; // used for storing movement 
    private bool onPlatformStay;

    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb2d = GetComponent<Rigidbody2D>();
        onPlatformStay = false;
    }

    // FixedUpdate is used for physics simulations
    void FixedUpdate()
    {
        movement = Input.GetAxis("Player 1 - Horizontal");
        if (movement != 0)
        {
            rb2d.velocity = new Vector2(speed * movement, rb2d.velocity.y); // move left/right
        }

        animator.SetFloat("speed", Mathf.Abs(movement)); // changes the animation of the sprite to running

        // invert the sprites image to simulate turning left and right
        if (movement > 0f && !facingRight) // if not looking right... look right
        {
            Flip();
        }
        if (movement < 0f && facingRight) //if not looking left... look left
        {
            Flip();
        }
    }

    void Update()
    {
        //jumping mechanism to simulate a more retro game like gravity
        if (rb2d.velocity.y < 0) // if the player is falling down it will increase the gravity
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump")) // if the player small jumps it has a separate gravity speed
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowMultiplier - 1) * Time.deltaTime;
        }

        //the player can't go out of the right screen view 
        if (this.transform.position.x > 8.20)
        {
            float posy = this.transform.position.y;
            this.transform.position = new Vector3(8.20f, posy, 0);
        }

        if (IsGrounded()) // set jumpValue when the player is grounded
        {
            extraJumps = extraJumpsValue;
        }
        
        if (Input.GetButtonDown("Jump") && extraJumps > 0) // jump mechanism
        {
            rb2d.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetButtonDown("Jump") && (extraJumps == 0) && IsGrounded())
        {
            rb2d.velocity = Vector2.up * jumpForce;
        }


        // just a fun piece of code. It changes the color of the player when the " " key pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    bool IsGrounded() // check if the player is touching the ground
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, Ground);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    void Flip() // flip the image of the player to look in a certain direction
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision) // when player gets on the platform
    {
        if (collision.gameObject.name.Equals("platforms(Clone)"))
        {
            this.transform.SetParent(transform);
        }
    }

    void OnCollisionStay2D(Collision2D collision) // when the player stays on the platform
    {
        if (collision.gameObject.name.Equals("platforms(Clone)") && movement < 0 )
        {
            float offset = Mathf.Abs(this.rb2d.velocity.x - collision.rigidbody.velocity.x);
            rb2d.velocity = new Vector2(-offset * 2.70f, rb2d.velocity.y);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // when player leaves the platform
    {
        if (collision.gameObject.name.Equals("platforms(Clone)"))
        {
            this.transform.SetParent(null); 
        }
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.name.Equals("JumpUpgrade_01(Clone)")) // when player touches jumpUpgrade
        {
            // do something
        }

        if (col.gameObject.name.Equals("PotionUpgrade(Clone)")) // when player touches potionUpgrade
        {
            // do something
        }
    }

}
