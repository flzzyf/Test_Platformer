using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Effect_Force")]
public class Effect_Force : Effect
{
    public enum TargetType { Target, Caster }
    public TargetType targetType = TargetType.Target;

    public float amount = 3;
    public float time;

    public override void Trigger()
    {
        Controller2D controller = target.GetComponent<Controller2D>();

        controller.AddForce(target.transform.right, amount);

    }
}
