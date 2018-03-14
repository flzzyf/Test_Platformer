using UnityEngine;

public class Effect : ScriptableObject
{
    [HideInInspector]
    public GameObject target;

    public virtual void Trigger()
    {

    }
}
