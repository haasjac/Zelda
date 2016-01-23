using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

    public Text rupee_text;
    public Text key_text;
    public Text bomb_text;
    public Text health_text;
    public float speed = 0.1f;
    
    //Vector3 start;
    //Vector3 target;
    private string health;
    [HideInInspector]
    public bool paused;
    StateMachine hudMachine;

    // Use this for initialization
    void Start () {
        //start = transform.position;
        //target = new Vector3 ()
        paused = false;
        hudMachine = new StateMachine();
        hudMachine.ChangeState(new StateHudUnpaused(this));
    }
	
	// Update is called once per frame
	void Update () {
        rupee_text.text = "Rupees: " + PlayerControl.S.rupee_count.ToString();
        key_text.text = "Keys: " + PlayerControl.S.key_count.ToString();
        bomb_text.text = "Bombs: " + PlayerControl.S.bomb_count.ToString();
        health = "";
        for (int i =0; i < Mathf.FloorToInt(PlayerControl.S.health); i++) {
            health += "<3";
        }
        if (PlayerControl.S.health - Mathf.FloorToInt(PlayerControl.S.health) != 0) {
            health += "<";
        }
        health_text.text = health;
        if(Input.GetButtonDown("Start")) {
            if (!paused) {
                paused = true;
                hudMachine.ChangeState(new StateHudTrans(this, 5));
            }
            else {
                paused = false;
                hudMachine.ChangeState(new StateHudTrans(this, 585));
            }
        }
        hudMachine.Update();
    }
}
