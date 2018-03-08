using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyHelper : MonoBehaviour {

    public float wingPowerInit = 3;
    float wingPower;

    public float wingCooldown = 0.5f;
    float wingCooldownCurrent;

    public float preferedHeight;

    void Start ()
    {
        wingPower = wingPowerInit;

        preferedHeight = transform.position.y;
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WingUp();

        }

        //if(wingCooldownCurrent > 0)
        //{
        //    wingCooldownCurrent -= Time.deltaTime;
        //}
        //else
        //{
        //    wingCooldownCurrent = wingCooldown;

        //    WingUp();

        //}

        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        preferedHeight += inputV;

        Debug.Log(preferedHeight);

        if(transform.position.y < preferedHeight)
        {
            WingUp();
        }
    }

    //扇翅膀
    void WingUp()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        GetComponent<Rigidbody2D>().AddForce(transform.up * wingPower, ForceMode2D.Impulse);
    }
}
