using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

    public float speed = 3;
    //跳跃高度和跳跃时间
    public float jumpHeight = 4;
    public float timeToJump = .4f;
    
    //空中地上加速度
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    Controller2D controller;

    float gravity;
    float jumpVelocity;

    Vector3 velocity;

    float velocityXSmoothing;

    Animator animator;
    GameObject gfx;


    void Start () {
        controller = GetComponent<Controller2D>();
        gfx = GameObject.Find("GFX");

        animator = gfx.GetComponent<Animator>();

        CalculateGravityAndJumpVelocity();
	}

    void FixedUpdate()
    {
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        //接触地面
        if (controller.collisions.below)
        {
            animator.SetBool("hopping", false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpVelocity;

                animator.SetBool("hopping", true);

            }
        }

        float targetVelocityX = inputH * speed;
        velocity.y += gravity * Time.deltaTime;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, 
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("walkSpeed", Mathf.Abs(inputH));
    }
    //根据跳跃高度和时间计算重力和速度
    void CalculateGravityAndJumpVelocity()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJump, 2);

        jumpVelocity = Mathf.Abs(gravity) * timeToJump;
    }

}
