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
            if (comboStatusCounter[i] > 0 && animState.normalizedTime >= 0.9f)
            {
                //Debug.Log("清零");

                SetComboStatus(i, 0);
            }

            if (comboStatusCounter[i] < combo[i].action.Length) //未完成连击所有动作
            {
                if (Input.GetKeyDown(combo[i].action[comboStatusCounter[i]].key))
                {
                    if (comboStatusCounter[i] == 0 || animState.normalizedTime > 0.3f)
                    {
                        StartCoroutine(DelayEffect(i, comboStatusCounter[i]));

                        comboStatusCounter[i]++;

                        //print(comboStatusCounter[i]);

                        SetComboStatus(i, comboStatusCounter[i]);

                    }
                }
            }
        }
    }

    IEnumerator DelayEffect(int _index, int _statusIndex)
    {
        yield return new WaitForSeconds(Time.deltaTime);

        animState = animator.GetCurrentAnimatorStateInfo(0);

        //if(_statusIndex != 0)
        //yield return new WaitForSeconds(animState.length);

        string animName = combo[_index].animStatusName + "_" + (_statusIndex + 1);
        //print(animName);

        while (!animState.IsName(animName))
        {
            //print(animState.normalizedTime);
            //animState = animator.GetCurrentAnimatorStateInfo(0);

            yield return new WaitForSeconds(Time.deltaTime);

        }

        //print(animName + " 执行");

        combo[_index].action[_statusIndex].effect.Trigger(gameObject);

    }

    void SetComboStatus(int _index, int _status)
    {
        comboStatusCounter[_index] = _status;

        animator.SetInteger(combo[_index].animStatusName, _status);

    }

}
