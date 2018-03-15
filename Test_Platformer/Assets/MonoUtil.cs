using UnityEngine;

public class MonoUtil : MonoBehaviour
{
    public static MonoUtil instance;

    void Awake()
    {
        MonoUtil.instance = this;
    }
}