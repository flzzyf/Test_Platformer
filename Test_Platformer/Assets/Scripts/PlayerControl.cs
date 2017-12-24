using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Range(1, 10)]
    public float jumpVelocity = 10;

    public float speed = 3;

    Rigidbody rb;

    Animator animator;

    GameObject gfx;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        gfx = GameObject.Find("GFX");

        animator = gfx.GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !hopping)
        {
            GetComponent<Rigidbody>().velocity = Vector3.up * jumpVelocity;

            animator.SetBool("hopping", true);
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();

        if(hopping)
            GravityJump();

    }

    void PlayerMove()
    {
        float inputV = Input.GetAxis("Vertical");
        float inputH = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * inputH * speed * Time.deltaTime, Space.World);

        animator.SetFloat("walkSpeed", inputH);
    }

    void GravityJump()
    {
        //下落时（或上升中放开空格）附加重力
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public bool hopping = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider);
        //Debug.Log(collision.rigidbody);

        hopping = false;

        animator.SetBool("hopping", false);


    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit");

        hopping = true;
    }
}
