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

        for (int i = 0; i < combo.Length; i++)
        {
            if (Input.GetKeyDown(combo[i].action[comboStatusCounter[i]].key))
            {
                print("key");

                if (comboStatusCounter[i] == 0 || animState.normalizedTime > 0.3f)
                {
                    print("key2");

                    if (comboStatusCounter[i] < combo[0].action.Length) //未完成连击所有动作
                    {
                        //comboStatusCounter[i]++;

                        //print(comboStatusCounter[i]);

                        SetComboStatus(i, ++comboStatusCounter[i]);
                    }

                }
            }

            if (comboStatusCounter[i] > 0 && animState.normalizedTime >= 0.9f)
            {
                Debug.Log("清零");

                //SetComboStatus(i, 0);


            }
        }

    }

    void SetComboStatus(int _index, int _status)
    {
        comboStatusCounter[_index] = _status;

        animator.SetInteger(combo[_index].animStatusName, _status);

        print("" + _index + _status);

    }

}
