using UnityEngine;
using System.Collections;

public class Aquamentus : MonoBehaviour {
    //TACTICS: -attack at every interval
    //         -slowly approach link
    //         -back up a little if damaged
    public GameObject link;
    public GameObject orb;
    public int health = 10;
    public float movement_speed = 1f;
    public float distance = 0.0f;
    public float attack_cooldown = 0.0f;
    public Direction horizontal = Direction.WEST;
    public Direction vertical = Direction.NORTH;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (attack_cooldown <= 0.0f) { 
            attack();
            attack_cooldown = 1f;
            change_horizontal();
        }
        else {
            Vector3 pos = this.transform.position;
            float shift = Time.deltaTime * movement_speed;
            //move and decrease cooldown
            if (horizontal == Direction.EAST)
                pos.x += shift;
            else if (horizontal == Direction.WEST)
                pos.x -= shift;

            if (vertical == Direction.NORTH)
                pos.y += shift / 2;
            else if (vertical == Direction.SOUTH)
                pos.y -= shift / 2;

            attack_cooldown -= Time.deltaTime;
        }
	}


    void attack() {
        //instantiate 3 orbs and hurl them at the player
    }

    void change_horizontal() {
        int dir = Mathf.FloorToInt(Random.Range(0f, 3f));
        if (dir == 0)
            vertical = Direction.NORTH;
        else if (dir == 1)
            vertical = Direction.SOUTH;
        else
            vertical = Direction.EAST;
    }
}
