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
        if (targetType == TargetType.Target)    //对目标释放
        {
            Controller2D controller = target.GetComponent<Controller2D>();

            Vector2 dir = target.transform.position - caster.transform.position;

            if (controller != null)
            {
                controller.AddForce(dir * amount, amount);

            }
            else
            {
                //target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                target.GetComponent<Rigidbody2D>().AddForce(dir * amount, ForceMode2D.Impulse);   //没有controller就直接rb施力 

            }
        }
        else
        {
            Controller2D controller = caster.GetComponent<Controller2D>();
            controller.AddForce(caster.transform.right, amount);

        }
    }
}
