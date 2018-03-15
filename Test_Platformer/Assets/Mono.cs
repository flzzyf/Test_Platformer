using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono : MonoBehaviour
{
    public static Mono instance;

    void Awake()
    {
        Debug.Log("qwe");
        Mono.instance = this;
    }
}