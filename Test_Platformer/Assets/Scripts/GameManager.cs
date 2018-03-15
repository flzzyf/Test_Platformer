using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Effect effect;

    public GameObject caster, target;

	void Start () {
		
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            //effect.target = gameObject;
            //effect.Trigger();

            effect.Trigger(caster, target);
        }
	}

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
