using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Effect/Effect_Set")]
public class Effect_Set : Effect
{
    public enum TargetPosType { Null, Point, GameObject }
    public TargetPosType targetPosType = TargetPosType.GameObject;

    public Effect[] effects = new Effect[1];

    public float delayTime;
    float timer;

    public override void Trigger()
    {
        MonoUtil.instance.StartCoroutine(Delay());

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);

        foreach (Effect item in effects)
        {
            SetupChild(item);
            item.Trigger();
        }
    }
}
