using UnityEngine;

public class Effect : ScriptableObject
{
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public GameObject caster;

    public virtual void Trigger()
    {

    }

    public virtual void SetupChild(Effect _effect)
    {
        _effect.target = target;
        _effect.caster = caster;
    }
}
