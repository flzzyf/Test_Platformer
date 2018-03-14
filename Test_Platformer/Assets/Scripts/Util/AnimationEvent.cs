using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour {

    public UnityEvent girl_Combo_ZZZ;

    Animator animator;

    AnimatorStateInfo animState;

    private void Start()
    {
        animator = GetComponent<Animator>();

        animState = animator.GetCurrentAnimatorStateInfo(0);
    }

    public void Girl_Combo_ZZZ()
    {
        girl_Combo_ZZZ.Invoke();

    }

    public void AddForce()
    { 
        
    }
}
