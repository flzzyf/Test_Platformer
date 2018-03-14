using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerControl : MonoBehaviour {
    
    Controller2D controller;

    Animator animator;
    public GameObject gfx;

    SpriteRenderer sprite;

    float inputH;

    public Collider2D collider;

    void Start () {
        controller = GetComponent<Controller2D>();
        sprite = gfx.GetComponent<SpriteRenderer>();

        animator = gfx.GetComponent<Animator>();

	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            controller.AddForce(transform.right, 6);
        }

        if (Input.GetKey(KeyCode.F))
        {
            return;
        }

        inputH = Input.GetAxisRaw("Horizontal");

        //float inputV = Input.GetAxisRaw("Vertical");

        if (inputH != 0 && sprite != null)
        {
            Flip(controller.collisions.facing);

        }

        //滑墙动画
        animator.SetBool("sliding", controller.wallSliding);


        //接触地面
        if (controller.collisions.below)
        {
            if (animator != null)
                animator.SetBool("jumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))    //按空格
        {
            StartCoroutine(Jump());

        }


        //移动
        //行走动画
        if(animator != null)
            animator.SetFloat("speed", Mathf.Abs(inputH));

        //特殊跳跃机制
        //GravityJump();

        if (!controller.collisions.below && !controller.wallSliding && !animator.GetBool("jumping")) 
        {
            //Debug.Log("自然掉落");
            animator.SetBool("jumping", true);
        }
    }

    private void FixedUpdate()
    {
        controller.PreMove(inputH);

    }

    void Flip(int _dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = _dir;
        transform.localScale = scale;
    }


    //特殊跳跃机制
    /*
    void GravityJump()
    {
        if(velocity.y > 0 && !Input.GetButton("Jump"))
        {
            //在上升中没按着跳跃键，则附加重力加速落地
            velocity += Vector3.up * gravity * Time.deltaTime;
        }
    }*/

    IEnumerator Jump()
    {
        //跳跃
        //jumpCount--;
        if (controller.Jump())
        {
            if (animator != null)
            {
                animator.SetBool("jumping", false);
                yield return new WaitForSeconds(Time.deltaTime);
                animator.SetBool("jumping", true);

            }
        }
    }

    public void Girl_Combo_ZZZ(int _index)
    {
        ComboControl comboControl = GetComponent<ComboControl>();

        comboControl.combo[0].action[_index].effect.target = gameObject;
        comboControl.combo[0].action[_index].effect.Trigger();

        foreach (GameObject item in list)
        {
            print(item.name);
            item.GetComponent<Rigidbody2D>().AddForce(transform.right * 3, ForceMode2D.Impulse);

        }
    }

    List<GameObject> list = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        list.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        list.Remove(collision.gameObject);
    }
}
