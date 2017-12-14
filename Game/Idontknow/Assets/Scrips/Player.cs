﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Rigidbody
    private Rigidbody rb;

    //X movement
    public float speed;
    public float minSpeed;
    public float maxSpeed;

    //drag
    public float drag;
    public bool airDragTest;

    //Y movement
    public float jumpSpeed;
    public float airJumpMultiplier;
    public int maxAirJumps;
    int airJumps;
    
    //gravity
    public float gravity;
    public float wallGravity;
    public float fallingMultiplier;
    public float lowJumpMultiplier;
    
    //collision ditection
    private Collider col;
    private float distToGround;
    private float distToWall;

	// Use this for initialization
	void Start () {
        //Get the nessesary components
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
        // get the distance to ground
        distToGround = col.bounds.extents.y;
        distToWall = col.bounds.extents.x;
    }

    //Checks if the player is on the ground using 5 raycast to minimize the area not checked and returns a boolean
    bool IsGrounded(){
        bool OnGround1 = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        bool OnGround2 = Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x/2 + 0.01f, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround3 = Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x/2 - 0.01f, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround4 = Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround5 = Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x / 4, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround = (OnGround1 || OnGround2 || OnGround3 || OnGround4 || OnGround5);
        return OnGround;
    }

    //Checks if the player is on the left wall
    bool OnLeftWall()
    {
        bool OnWall1 = Physics.Raycast(transform.position, Vector3.left, distToWall + 0.1f);
        bool OnWall = OnWall1;
        return OnWall;
    }

    //Checks if the player is on the right wall
    bool OnRightWall()
    {
        bool OnWall1 = Physics.Raycast(transform.position, Vector3.right, distToWall + 0.1f);
        bool OnWall = OnWall1;
        return OnWall;
    }

    // Update is called once per frame
    void Update()
    {
        //physics
        {
            //Simulate gravity for player only 
            //Gravity on a wall is lower while moving down
            if ((OnLeftWall() || OnRightWall()) && (rb.velocity.y < 0))
            {
                rb.AddForce(0, -wallGravity, 0);
            }
            else
            {
                //Change gravity strength to make jumps less floaty
                if (rb.velocity.y < 0)
                {
                    rb.AddForce(0, -gravity * fallingMultiplier, 0);
                }
                //Holding up makes you jump higher
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    rb.AddForce(0, -gravity, 0);
                }
                //Higher gravity when not holding up
                else
                {
                    rb.AddForce(0, -gravity * lowJumpMultiplier, 0);
                }
            }

            //Limit the speed a player can move at
            if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector3(maxSpeed, rb.velocity.y, 0);
            }
            if (rb.velocity.x < -maxSpeed)
            {
                rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, 0);
            }

            //If no or both directions are pressed slow down using drag
            if (!((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.LeftArrow))) || ((Input.GetKey(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.LeftArrow))))
            {
                //Minimal speed to avoid micromovements instead of stopping
                if (rb.velocity.x < minSpeed && rb.velocity.x > -minSpeed)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }

                //Apply drag when on ground or when airDrag is turned on
                if (IsGrounded() || airDragTest)
                {
                    if (rb.velocity.x >= minSpeed)
                    {
                        rb.AddForce(-drag, 0, 0);
                    }
                    if (rb.velocity.x <= -minSpeed)
                    {
                        rb.AddForce(drag, 0, 0);
                    }
                }
            }
        }

        //Reset doublejumps when on the ground
        if (IsGrounded())
        {
            airJumps = maxAirJumps;
        }

        //Controls
        {
            //Movement controls
            if (Input.GetKey(KeyCode.LeftArrow) && !(Input.GetKey(KeyCode.RightArrow)))
            {
                rb.AddForce(-speed, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow) && !(Input.GetKey(KeyCode.LeftArrow)))
            {
                rb.AddForce(speed, 0, 0);
            }

            //Press up to jump
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //Check if the player not on a wall is in the air or if the player is on the ground
                if (!(OnLeftWall() || OnRightWall()) || IsGrounded()) {
                    //Jumping while on the ground is higher
                    if (IsGrounded())
                    {
                        rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.VelocityChange);
                    }
                    //Check if any air jumps are remaining and airjumps are effected by airjump multiplier
                    else if (airJumps > 0)
                    {
                        rb.AddForce(new Vector3(0, jumpSpeed * airJumpMultiplier, 0) - new Vector3(0, rb.velocity.y, 0), ForceMode.VelocityChange);
                        airJumps--;
                    }
                }
                //If on a wall in the air do a walljump
                else
                {
                    //On Left wall jump right
                    if (OnLeftWall())
                    {
                        rb.AddForce(new Vector3(jumpSpeed, jumpSpeed, 0) - new Vector3(0, rb.velocity.y, 0), ForceMode.VelocityChange);
                    }
                    //On right wall jumo left
                    if (OnRightWall())
                    {
                        rb.AddForce(new Vector3(-jumpSpeed, jumpSpeed, 0) - new Vector3(0, rb.velocity.y, 0), ForceMode.VelocityChange);
                    }
                }
            }
            //While in the air press down to cancel jump and fall down faster
            if ((Input.GetKeyDown(KeyCode.DownArrow)) && !IsGrounded())
            {
                //Limit the downwards speed achieved with downfall
                if (rb.velocity.y > (-3 * jumpSpeed))
                {
                    rb.velocity = new Vector3(rb.velocity.x, -3 * jumpSpeed, 0);
                }
            }
        }
     }
}
