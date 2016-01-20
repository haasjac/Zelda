using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float movement_speed = 1f;
    public float dir_cooldown = 2.5f;
    public float attack_cooldown = 1.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	//move in a random direction every dir_cooldown     //make sure enemies can't go through doors!!!

	}

    public void change_dir() {
        int direction = Random.Range(0, 3);

    }

    void OnCollisionEnter(Collision coll) {
        //if you collide with a solid object, change direction
    }
}
