using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerControl : MonoBehaviour {

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

    SpriteRenderer sprite;

    void Start () {
        controller = GetComponent<Controller2D>();
        gfx = GameObject.Find("GFX");
        sprite = gfx.GetComponent<SpriteRenderer>();

        animator = gfx.GetComponent<Animator>();

        CalculateGravityAndJumpVelocity();
	}

    void FixedUpdate()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        //float inputV = Input.GetAxisRaw("Vertical");

        if(inputH != 0 && sprite != null)
        {
            //float directionH = Mathf.Sign(inputH);

            if(inputH > 0)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;

            }
        }

        //上下方有物体时重置y速度
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        //接触地面
        if (controller.collisions.below)
        {
            if(animator != null)
            animator.SetBool("hopping", false);
            //跳跃
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = jumpVelocity;
                if (animator != null)

                    animator.SetBool("hopping", true);
            }
        }
        //计算速度
        float targetVelocityX = inputH * speed;
        velocity.y += gravity * Time.deltaTime;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, 
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        //移动
        controller.Move(velocity * Time.deltaTime);
        //行走动画
        if(animator != null)
            animator.SetFloat("walkSpeed", Mathf.Abs(inputH));

        //特殊跳跃机制
        GravityJump();

    }

    //根据跳跃高度和时间计算重力和速度
    void CalculateGravityAndJumpVelocity()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJump, 2);

        jumpVelocity = Mathf.Abs(gravity) * timeToJump;
    }

    //特殊跳跃机制
    void GravityJump()
    {
        if(velocity.y > 0 && !Input.GetButton("Jump"))
        {
            //在上升中没按着跳跃键，则附加重力加速落地
            velocity += Vector3.up * gravity * Time.deltaTime;
        }
    }

}
