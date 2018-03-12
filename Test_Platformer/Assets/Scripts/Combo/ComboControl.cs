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

    Dictionary<int, string> info = new Dictionary<int, string>();

    public Text infoText;

    private void Start()
    {
        //初始化
        for (int i = 0; i < combo.Length; i++)
        {
            timer[i] = 0;
            counter[i] = 0;
            info[i] = "";
        }
    }

    void Update()
    {
        for (int i = 0; i < combo.Length; i++)
        {
            //按键判定
            if (Input.GetKeyDown(combo[i].action.key[counter[i]]))
            {
                info[i] += combo[i].action.key[counter[i]];
                counter[i]++;
                //按键显示
                infoText.text = "按键输入：" + info[i];

                if (counter[i] == 0)
                {
                    timer.Add(i, Time.time);

                }
                else
                {
                    if(counter[i] == combo[i].action.key.Length)
                    {
                        //Debug.Log("触发");
                        combo[i].effect.Trigger();
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
            if (counter[i] != 0 && Time.time - timer[i] > combo[i].action.delayTime)
                ComboReset(i);

        }
    }

    void ComboReset(int _index)
    {
        //Debug.Log("清零");
        timer[_index] = 0;
        counter[_index] = 0;
        info[_index] = "";
    }

}
