using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour {

    public UnityEvent explode;

    public void Event_Explode()
    {
        //Debug.Log("meg");

        explode.Invoke();
    }
}
