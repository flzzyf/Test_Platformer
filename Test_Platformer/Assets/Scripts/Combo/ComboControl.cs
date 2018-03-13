using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboControl : MonoBehaviour {

    public Combo[] combo;
    //连招计时器
    Dictionary<int, float> timer = new Dictionary<int, float>();
    //连招计数器
    Dictionary<int, int> counter = new Dictionary<int, int>();

    AnimatorStateInfo animState;

    public Animator animator;

    int comboStatus = 0;

    private void Start()
    {
        //初始化
        for (int i = 0; i < combo.Length; i++)
        {
            timer[i] = 0;
            counter[i] = 0;
        }

    }

    void Update()
    {
        /*
        for (int i = 0; i < combo.Length; i++)
        {
            //按键判定
            if (Input.GetKeyDown(combo[i].action[counter[i]].key))
            {
                counter[i]++;

                if (counter[i] == 0)
                {
                    timer.Add(i, Time.time);

                }
                else
                {
                    if(counter[i] == combo[i].action.Length)
                    {
                        Debug.Log("触发");
                        combo[i].action[counter[i]].effect.Trigger();
                        ComboReset(i);

                    }
                    else
                    {
                        //重置当前时间
                        timer[i] = Time.time;

                    }
                }
            }

            //连招超时清零
            if (counter[i] != 0 && Time.time - timer[i] > combo[i].action[counter[i]].delayTime)
                ComboReset(i);
        
        }*/

        animState = animator.GetCurrentAnimatorStateInfo(0);

        if (comboStatus > 0 && animState.normalizedTime >= 0.9f)
        {
            Debug.Log("清零");

            comboStatus = 0;
            animator.SetInteger("combo_ZZZ", 0);

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (animState.normalizedTime > 0.3f)
            {
                comboStatus++;

                Debug.Log(comboStatus);

                animator.SetInteger("combo_ZZZ", comboStatus);

            }
        }

 

    }

    void ComboReset(int _index)
    {
        //Debug.Log("清零");
        timer[_index] = 0;
        counter[_index] = 0;
    }

}
