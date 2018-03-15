using UnityEngine;

[System.Serializable]
public class Filter
{
    public FilterItem[] filterItems = {
        new FilterItem(FilterManager.flagName[0]),
        new FilterItem(FilterManager.flagName[1]),
        new FilterItem(FilterManager.flagName[2]),
        new FilterItem(FilterManager.flagName[3]),
        new FilterItem(FilterManager.flagName[4])
    };
}
