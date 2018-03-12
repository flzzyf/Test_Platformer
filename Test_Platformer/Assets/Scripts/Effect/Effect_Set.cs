using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_Set")]
public class Effect_Set : Effect
{
    public Effect[] effects;

    public string text;

    public override void Trigger()
    {
        //Debug.Log("效果集触发：" + text);

        foreach (var item in effects)
        {
            item.Trigger();
        }
    }
}
