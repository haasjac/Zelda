using UnityEngine;
using System.Collections;

public class Aquamentus : MonoBehaviour {
    //TACTICS: -attack at every interval
    //         -slowl move back and forth (horizontally)

    //camera tracking
    public GameObject cam;

    //attack variables
    public GameObject link;
    public GameObject orb;
    public Vector3 target_middle;
    public Vector3 target_left;
    public Vector3 target_right;
    public float attack_cooldown = 2f;

    //movement variables
    public int health = 10;
    public float distance = 2f;
    public float movement_speed = 1f;
    public float stun = 0.0f;                                   //can aquamentus be stunned?
    public Direction horizontal = Direction.WEST;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!Utils.on_camera(this.gameObject, cam))
            return;

        if(stun > 0.0f) {
            stun -= Time.deltaTime;
            return;
        }

        //direction update
        if(distance <= 0.0f) {
            distance = 2f;
            if (horizontal == Direction.EAST)
                horizontal = Direction.WEST;
            else
                horizontal = Direction.EAST;
        }

        //attack and movement updates
        if (attack_cooldown <= 0.0f) {
            attack();
            attack_cooldown = 2.5f;
        }
        else {
            Vector3 pos = this.transform.position;
            float shift = Time.deltaTime * movement_speed;
            //move and decrease cooldown
            if (horizontal == Direction.EAST)
                pos.x += shift;
            else if (horizontal == Direction.WEST)
                pos.x -= shift;

            distance -= shift;
            attack_cooldown -= Time.deltaTime;
            this.transform.position = pos;
        }
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


    void OnCollisionEnter(Collision coll) {
        switch (coll.gameObject.tag) {
            case "PlayerProjectile":
                health -= 1;
                if (health <= 0)
                    Destroy(this.gameObject);
                break;
            case "Boomerang":
                stun = 2.5f;
                break;
        }
    }
}
