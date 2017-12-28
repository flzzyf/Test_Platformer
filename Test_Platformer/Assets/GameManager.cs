using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}
    public Texture btnTexture;

    void OnGUI()
    {
        //带贴图的按钮
        if (GUI.Button(new Rect(10, 10, 50, 50), btnTexture))
            Debug.Log("Clicked the button with an image");

        //带文本按钮
        if (GUI.Button(new Rect(10, 70, 100, 30), "重新开始"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
