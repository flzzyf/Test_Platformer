using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Effect_Set")]
public class Effect_Set : Effect
{
    public enum TargetPosType { Null, Point, GameObject }
    public TargetPosType targetPosType = TargetPosType.GameObject;

    public Effect[] effects = new Effect[1];

    public override void Trigger()
    {
        foreach (var item in effects)
        {
            item.Trigger();
        }
    }
}
