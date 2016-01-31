using UnityEngine;
using System.Collections;

public enum Condition { OnKillOpen, OnKillSpawn, OnPushOpen}

public class Room_tracker : MonoBehaviour {

    public Condition con;
    public GameObject focus;
    public GameObject block;
    public GameObject[] enemies;
    public Sprite door_sprite;

    public bool all_clear = true;
    public bool reset_on_enter = false;

    public Vector3 block_pos = Vector3.zero;
    public Sprite orig_sprite;

	// Use this for initialization
	void Start () {
	    if(con == Condition.OnPushOpen) {
            block_pos = block.transform.position;
            orig_sprite = focus.GetComponent<SpriteRenderer>().sprite;
        }
	}
	
	// Update is called once per frame
	void Update () {
        switch (con) {
            case Condition.OnKillOpen:
                //check the array of enemies
                for (int i = 0; i < enemies.Length; i++) {
                    if (enemies[i] != null)
                        all_clear = false;
                    else
                        all_clear = true;
                }
                if (all_clear)
                    open(focus);
                break;
            case Condition.OnKillSpawn:
                for (int i = 0; i < enemies.Length; i++) {
                    if (enemies[i] != null)
                        all_clear = false;
                    else
                        all_clear = true;
                }
                //spawn the object in the center of the room
                if (all_clear) {
                    Instantiate<GameObject>(focus);
                    focus.transform.position = this.transform.position;
                }
                break;
            case Condition.OnPushOpen:
                if (block.gameObject.tag == "Pushable")
                    break;
                open(focus);
                break;
        }
	}

    void OnTriggerEnter(Collider coll) {
        switch (coll.gameObject.tag) {
            case "Player":
                if (reset_on_enter)
                    break;
//                    reset();                                          //TESTING REQUIRED!!!
                break;
        }
    }

    void OnTriggerExit(Collider coll) {
        switch (coll.gameObject.tag) {
            case "Player":
                reset_on_enter = true;
                break;
        }
    }

    void open(GameObject door) {
        if (door.tag != "Door") {
            //change sprite, tag, and collider
            door.tag = "Door";
            door.GetComponent<SpriteRenderer>().sprite = door_sprite;
            door.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void reset() {
        if(con == Condition.OnPushOpen) {
            block.gameObject.tag = "Pushable";
            block.transform.position = block_pos;

            BoxCollider focus_coll = focus.gameObject.GetComponent<BoxCollider>();
            focus_coll.isTrigger = false;
            focus_coll.size = Vector3.one;
            focus_coll.center = Vector3.zero;
            focus.gameObject.tag = "Trigger_door";
            focus.GetComponent<SpriteRenderer>().sprite = orig_sprite;
        }
    }
}
