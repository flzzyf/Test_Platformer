using UnityEngine;
using System.Collections;

[System.Serializable]
public class FilterItem
{
    [HideInInspector]
    public string name;
    public bool Is;

    public FilterItem(string _name)
    {
        name = _name;
    }
}
