using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyHelper : MonoBehaviour {

    public float wingPowerInit = 3;
    float wingPower;

    public float wingCooldown = 0.2f;
    float wingCooldownCurrent;

    public Vector2 targetPoint;
    public Transform target;

    public float flyHeightOffset = 0.5f;

    public float followDistanceX = 3;

    Vector2 currentPos;

    void Start ()
    {
        wingPower = wingPowerInit;

        targetPoint = transform.position;
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WingUp();

        }

        if(wingCooldownCurrent > 0)
        {
            wingCooldownCurrent -= Time.deltaTime;
        }

        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.F))
        {
            target = null;


            currentPos = transform.position;

            followDistanceX = 0;

        }
        Debug.Log(inputV);

        if (Input.GetKey(KeyCode.F))
        {
            currentPos += new Vector2(inputH, inputV) * Time.deltaTime * 5;
            targetPoint = currentPos;

        }


        if (Input.GetKeyUp(KeyCode.F))
        {

        }

    }

    void FixedUpdate()
    {
        FollowTarget();

    }

    //扇翅膀
    void WingUp()
    {
        wingCooldownCurrent = wingCooldown;

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        GetComponent<Rigidbody2D>().AddForce(Vector2.up * wingPower, ForceMode2D.Impulse);
    }

    void FollowTarget()
    {
        if(target != null)
        {
            targetPoint = target.position;
        }

        //高度控制
        if (transform.position.y < targetPoint.y + flyHeightOffset)
        {
            if (wingCooldownCurrent <= 0)
            {
                WingUp();
            }
        }
        
        if(transform.position.x < targetPoint.x - followDistanceX || transform.position.x > targetPoint.x + followDistanceX)
        {
            float dir = targetPoint.x - transform.position.x;

            transform.Translate(Vector2.right * dir * Time.deltaTime);
        }

    }
}
