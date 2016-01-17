using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

    public Text rupee_text;
    public Text key_text;
    public Text bomb_text;
    public Text health_text;
    private string health;

    // Use this for initialization
    void Start () {
	
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
    }
}
