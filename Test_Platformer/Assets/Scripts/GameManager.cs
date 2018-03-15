using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Effect effect;

	void Start () {
		
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            effect.Trigger();
        }
	}

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
