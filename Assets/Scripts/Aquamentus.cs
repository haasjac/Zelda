using UnityEngine;
using System.Collections;

public class Aquamentus : MonoBehaviour {
    //TACTICS: -attack at every interval
    //         -slowly approach link
    //         -back up a little if damaged
    public GameObject link;
    public GameObject orb;
    //public GameObject middle;
    //public GameObject left;
    //public GameObject right;
    public Vector3 target_middle;
    public Vector3 target_left;
    public Vector3 target_right;
    public int health = 10;
    public float movement_speed = 1f;
//    public float flee_cooldown = 0.0f;
//    public float attack_cooldown = 0.0f;
    public Direction horizontal = Direction.WEST;
    public Direction vertical = Direction.NORTH;

    // Use this for initialization
    void Start () {
        InvokeRepeating("attack", 2f, 2.5f);
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //direction update
        direction_update();

        //attack and movement updates
        //if (attack_cooldown <= 0.0f) { 
        //    attack();
        //    attack_cooldown = 1f;
        //}
//        else {
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

//            attack_cooldown -= Time.deltaTime;
            this.transform.position = pos;
//        }
	}


    void attack() {
        print("orbs away!");
        Vector3 link_pos = link.transform.position;
        //instantiate 3 orbs and hurl them at the player
        //extend x value to ensure each orb collides with something
        if (link_pos.x < transform.position.x)
            link_pos.x -= 10f;
        else
            link_pos.x += 10f;

        target_middle = link_pos;
        link_pos.y += 3f;
        target_right = link_pos;
        link_pos.y -= 6f;
        target_left = link_pos;

        //instance location
        Vector3 pos = transform.position;
        pos.x -= 0.75f;
        pos.y += 0.75f;
        GameObject middle = Instantiate<GameObject>(orb);               //orbs are triggers, MUST add OnTrigger damage
        middle.transform.position = pos;
        middle.GetComponent<orb_projectile>().target = target_middle;
        GameObject left = Instantiate<GameObject>(orb);
        left.transform.position = pos;
        left.GetComponent<orb_projectile>().target = target_left;
        GameObject right = Instantiate<GameObject>(orb);
        right.transform.position = pos;
        right.GetComponent<orb_projectile>().target = target_right;
    }

    void direction_update() {
        Vector3 link_pos = link.transform.position;
        Vector3 pos = this.transform.position;

        if (link_pos.x > pos.x)
            horizontal = Direction.EAST;
        else if (link_pos.x < pos.x)
            horizontal = Direction.WEST;

        if (link_pos.y > pos.y)
            vertical = Direction.NORTH;
        else if (link_pos.y < pos.y)
            vertical = Direction.SOUTH;
    }

    void OnCollisionEnter(Collision coll) {
        switch (coll.gameObject.tag) {
            case "PlayerProjectile":
                health -= 1;
                if (health <= 0)
                    Destroy(this.gameObject);
                break;
        }
    }
}
