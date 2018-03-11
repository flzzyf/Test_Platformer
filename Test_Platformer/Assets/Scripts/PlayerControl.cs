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

    public float wallSlideSpeedMax = 1;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallStickTime = .25f;
    float timeToWallUnstick;

    void Start () {
        controller = GetComponent<Controller2D>();
        gfx = GameObject.Find("GFX");
        sprite = gfx.GetComponent<SpriteRenderer>();

        animator = gfx.GetComponent<Animator>();

        CalculateGravityAndJumpVelocity();

	}

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            return;
        }

        float inputH = Input.GetAxisRaw("Horizontal");
        //float inputV = Input.GetAxisRaw("Vertical");

        float targetVelocityX = inputH * speed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        if (inputH != 0 && sprite != null)
        {
            //float directionH = Mathf.Sign(inputH);

            if (Mathf.Sign(inputH) != transform.localScale.x)
                Flip();

        }

        int wallDir = (controller.collisions.left) ? -1 : 1;

        bool wallSliding = false;
        if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
                velocity.y = -wallSlideSpeedMax;

            if(timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if(inputH != wallDir && inputH != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;

                    Debug.Log(timeToWallUnstick);
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
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
            OnGround();
        }

        if (Input.GetKeyDown(KeyCode.Space))    //按空格
        {
            Debug.Log("Space");
            if (jumpCount > 0)
            {

            }
            if (wallSliding)
            {
                if (wallDir == inputH)  //按朝着墙的方向
                {
                    velocity.x = -wallDir * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                    StartCoroutine(Jump(velocity));
                }
                else if (inputH == 0){  //不按方向键
                    velocity.x = -wallDir * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                    StartCoroutine(Jump(velocity));
                }
                else 
                {
                    //按和墙反方向键
                    velocity.x = -wallDir * wallLeap.x;
                    velocity.y = wallLeap.y;
                    StartCoroutine(Jump(velocity));
                }

            }
            if (controller.collisions.below)
            {
                velocity.y = jumpVelocity;
                StartCoroutine(Jump(velocity));

            }
        }

        //计算速度

        velocity.y += gravity * Time.deltaTime;

        //移动
        controller.Move(velocity * Time.deltaTime);
        //行走动画
        if(animator != null)
            animator.SetFloat("speed", Mathf.Abs(inputH));

        //特殊跳跃机制
        GravityJump();


    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //根据跳跃高度和时间计算重力和速度
    void CalculateGravityAndJumpVelocity()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJump, 2);

        jumpVelocity = Mathf.Abs(gravity) * timeToJump;
    }

    public int jumpCountMax = 2;
    int jumpCount;

    //特殊跳跃机制
    void GravityJump()
    {
        if(velocity.y > 0 && !Input.GetButton("Jump"))
        {
            //在上升中没按着跳跃键，则附加重力加速落地
            velocity += Vector3.up * gravity * Time.deltaTime;
        }
    }

    void OnGround()
    {

        jumpCount = jumpCountMax;

        if (animator != null)
            animator.SetBool("jumping", false);

    }

    IEnumerator Jump(Vector3 _velocity)
    {
        //跳跃
        //jumpCount--;

        velocity = _velocity;
        if (animator != null)
        {
            animator.SetBool("jumping", false);
            yield return new WaitForSeconds(Time.deltaTime);
            animator.SetBool("jumping", true);

        }

    }

}
