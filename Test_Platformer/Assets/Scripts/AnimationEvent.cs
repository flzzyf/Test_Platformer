using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour {

    public void meg()
    {
        //Debug.Log("meg");

        transform.parent.gameObject.GetComponent<Player>().Explode();
    }
}
