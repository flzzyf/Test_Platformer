using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

    Controller2D controller;

    float gravity = -20;
    Vector3 velocity;

    public float speed = 3;
    public float jumpVelocity = 3;

	void Start () {
        controller = GetComponent<Controller2D>();
	}

    private void Update()
    {
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        velocity.y += gravity * Time.deltaTime;
        velocity.x = inputH * speed;

        controller.Move(velocity * Time.deltaTime);
    }

}
