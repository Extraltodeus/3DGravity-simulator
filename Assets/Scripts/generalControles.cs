using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generalControles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update () {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            variables.gravityConstant += 10;
            Debug.Log(variables.gravityConstant);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            variables.gravityConstant -= 10;
            Debug.Log(variables.gravityConstant);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            variables.gravityConstant = 0;
            Debug.Log(variables.gravityConstant);
        }

        if (Input.mouseScrollDelta.y != 0){
            if (Input.GetKey(KeyCode.LeftControl))
            {
                GUI.speed += Input.mouseScrollDelta.y * 100;
            }
            else
            {
                GUI.planetSize = utilities.clamp(variables.size + Input.mouseScrollDelta.y * 10, variables.planetMinSize, variables.planetMaxSize);
            }

            
        }

    }
}
