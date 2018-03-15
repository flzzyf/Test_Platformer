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
    
    public virtual void Trigger(GameObject _caster)
    {
        caster = _caster;
        Trigger();
    }

    public virtual void Trigger(GameObject _caster, GameObject _target)
    {
        caster = _caster;
        target = _target;
        Trigger();
    }

    public virtual void SetupChild(Effect _effect)
    {
        _effect.target = target;
        _effect.caster = caster;
    }
}
