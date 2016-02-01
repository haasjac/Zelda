using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.O)) {
            Application.LoadLevel("Dungeon");
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Application.LoadLevel("Custom_Level");
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.LoadLevel("Main_Menu");
        }
    }
}
