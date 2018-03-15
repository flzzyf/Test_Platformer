using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    Controller2D controller;

    // Use this for initialization
    void Start () {
        controller = GetComponent<Controller2D>();

    }

    void Update ()
    {
        controller.PreMove(0);

    }
}
