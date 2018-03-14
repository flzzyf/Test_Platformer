using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class MyIntEvent : UnityEvent<int> { }
public class AnimationEvent : MonoBehaviour {

    public MyIntEvent girl_Combo_ZZZ;

    UnityAction action;

    Animator animator;

    AnimatorStateInfo animState;
    public MyIntEvent m_MyEvent;

    private void Start()
    {
        animator = GetComponent<Animator>();

        animState = animator.GetCurrentAnimatorStateInfo(0);

        //girl_Combo_ZZZ.AddListener(qwe);

        m_MyEvent = new MyIntEvent();

        m_MyEvent.AddListener(qwe);
    }

    public void Girl_Combo_ZZZ_0()
    {
        girl_Combo_ZZZ.Invoke(0);

    }
    public void Girl_Combo_ZZZ_1()
    {
        girl_Combo_ZZZ.Invoke(1);
    }
    public void Girl_Combo_ZZZ_2()
    {
        girl_Combo_ZZZ.Invoke(2);
    }

    public void AddForce()
    { 
        
    }

    void qwe(int a)
    {

    }
}
