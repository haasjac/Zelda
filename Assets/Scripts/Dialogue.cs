using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    public Text hudText;
    public string speechText;
    public float letterPerSecond = 2f;
    bool inRoom;
    float timer;
    int stringPos;
    bool done;

	// Use this for initialization
	void Start () {
        inRoom = false;
        hudText.text = "";
        stringPos = 0;
        timer = 0;
        done = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (inRoom) {
            if (stringPos < speechText.Length) {
                timer += Time.deltaTime;
                if (timer > 1/letterPerSecond ) {
                    hudText.text += speechText.Substring(stringPos, 1);
                    stringPos++;
                    timer = 0;
                }
            } else {
                if (!done) {
                    PlayerControl.S.control_state_machine.ChangeState(new StateLinkNormalMovement(PlayerControl.S));
                    done = true;
                }
            }
        } else {
            hudText.text = "";
        }
	}

    void OnTriggerEnter(Collider coll) {
        if(coll.gameObject.tag == "Player") {
            done = false;
            inRoom = true;
            PlayerControl.S.control_state_machine.ChangeState(new StateLinkStunned(PlayerControl.S));
        }
    }

    void OnTriggerExit(Collider coll) {
        if (coll.gameObject.tag == "Player") {
            inRoom = false;
            stringPos = 0;
        }
    }
}
