using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboControl : MonoBehaviour {

    public Combo[] combo;

    //连招计数器
    Dictionary<int, int> comboStatusCounter = new Dictionary<int, int>();

    AnimatorStateInfo animState;

    public Animator animator;

    private void Start()
    {
        //初始化
        for (int i = 0; i < combo.Length; i++)
        {
            SetComboStatus(i, 0);
        }

    }

    void Update()
    {
        animState = animator.GetCurrentAnimatorStateInfo(0);

        float animPercent = (animState.normalizedTime % animState.length) / animState.length;

        print("动画百分比：" + animPercent);

        for (int i = 0; i < combo.Length; i++)
        {
            if (comboStatusCounter[i] > 0 && animPercent >= 0.9f)
            {
                Debug.Log("清零");

                SetComboStatus(i, 0);

            }

            if (Input.GetKeyDown(combo[i].action[comboStatusCounter[i]].key))
            {
                print("key");

                if (comboStatusCounter[i] == 0 || animPercent > 0.3f)
                {
                    print("key2");

                    comboStatusCounter[i]++;

                    if (comboStatusCounter[i] < combo[0].action.Length) //未完成连击所有动作
                    {

                        //print(comboStatusCounter[i]);

                        SetComboStatus(i, comboStatusCounter[i]);
                    }
                    else
                    {
                        SetComboStatus(i, 0);

                    }

                }

            }


        }

    }

    void SetComboStatus(int _index, int _status)
    {
        comboStatusCounter[_index] = _status;

        animator.SetInteger(combo[_index].animStatusName, _status);

        //print("" + _index + _status);

    }

}
