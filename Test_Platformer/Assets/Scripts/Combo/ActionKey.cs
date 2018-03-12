using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action")]
public class ActionKey : ScriptableObject {

    public KeyCode[] key = new KeyCode[1];

    public float delayTime = 0.3f;

}
