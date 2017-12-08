using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public float speed;
    public float jumpSpeed;
    private Rigidbody rb;

    float distToGround;
    Collider col;
    public bool test;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
       // get the distance to ground
        distToGround = col.bounds.extents.y;
    }

    bool IsGrounded(){
        bool OnGround1 = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        bool OnGround2 = Physics.Raycast(new Vector3(transform.position.x - transform.localScale.x/2, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround3 = Physics.Raycast(new Vector3(transform.position.x + transform.localScale.x/2, transform.position.y, transform.position.z), -Vector3.up, distToGround + 0.1f);
        bool OnGround = (OnGround1 || OnGround2 || OnGround3);
        return OnGround;
    }

	// Update is called once per frame
	void Update () {
        /*
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector3(-speed, rb.velocity.y, rb.velocity.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
        }
        if (!((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.LeftArrow))))
        {
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow)) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow)) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        }
        */

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(new Vector3(-speed, rb.velocity.y, 0) - rb.velocity, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(new Vector3(speed, rb.velocity.y, 0) - rb.velocity, ForceMode.VelocityChange);
        }
        if (!((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.LeftArrow))))
        {
            rb.AddForce(new Vector3(0, rb.velocity.y, 0) - rb.velocity, ForceMode.VelocityChange);
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow)) && IsGrounded())
        {
            rb.AddForce(new Vector3(rb.velocity.x, jumpSpeed, 0) - rb.velocity, ForceMode.VelocityChange);
        }
        if ((Input.GetKeyDown(KeyCode.DownArrow)) && !IsGrounded())
        {
            rb.AddForce(new Vector3(rb.velocity.x, -jumpSpeed, 0) - rb.velocity, ForceMode.VelocityChange);
        }
    }
}
