using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Image sword;
    int index;

	// Use this for initialization
	void Start () {
        index = 0;
	}

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown("Vertical")) {
            index++;
        }

        if(Input.GetButtonDown("Select")) {
            index++;
        }

        Vector3 pos = sword.GetComponent<RectTransform>().anchoredPosition;

        if (index % 2 == 0) {
            pos.y = 23.5f;
        } else if (index % 2 == 1) {
            pos.y = 7.5f;
        }

        sword.GetComponent<RectTransform>().anchoredPosition = pos;

        if (Input.GetButtonDown("Start")) {
            if (index % 2 == 0) {
                Application.LoadLevel("Dungeon");
            } else {
                Application.LoadLevel("Custom_Level");
            }
        }
    }
}
