using UnityEngine;
using System.Collections;

public class Keese : MonoBehaviour {

    public GameObject cam;
    public Vector3 origin;
    public Direction horizontal = Direction.NORTH;
    public Direction vertical = Direction.EAST;
    public float movement_speed = 3f;
    public float distance = 0;
    public float cooldown = 0;
    public float target_cooldown = 0f;
    public float animation_speed = 1f;
    public int health = 1;
    public bool rest = false;
//    public Animation moving;

	// Use this for initialization
	void Start () {
        origin = this.transform.position;
        target_cooldown = Random.Range(8, 15);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!Utils.check_movement(horizontal, this.gameObject, cam)) {
            this.transform.position = origin;
            return;
        }

        //cooldown update
        if (!rest) {
            cooldown += Time.deltaTime;
            if (cooldown >= target_cooldown) {
                rest = true;
                cooldown = 2.5f;
            }
        }

        if (rest) {                //if resting, don't move
            cooldown -= Time.deltaTime;
            change_dir();
            if (cooldown <= 0.0f)
                rest = false;
        }
        else if (distance <= 0.0f) {    //if finished moving, reset parameters
            distance = Random.Range(0, 4);
            change_dir();
        }
        else {                          //otherwise, move normally
            Vector3 pos = this.transform.position;
            float shift = Time.deltaTime * movement_speed;
            if (shift > distance)
                shift = distance;

            //horizontal
            if (horizontal == Direction.EAST)
                pos.x += shift;
            else if (horizontal == Direction.WEST)
                pos.x -= shift;

            //vertical
            if (vertical == Direction.NORTH)
                pos.y += shift;
            else if (vertical == Direction.SOUTH)
                pos.y -= shift;

            distance -= shift;
            this.transform.position = pos;

        }

        //update animation
        //if (distance <= 1f)
        //    moving["Keese_moving"].speed = distance;
        ////            GetComponent<Animation>()["Keese_moving"].speed = distance;
        //else
        //    moving["Keese_moving"].speed = 1;
        ////            GetComponent<Animation>()["Keese_moving"].speed = 1f;

        if (rest)
            GetComponent<Animator>().SetBool("moving", false);
        else
            GetComponent<Animator>().SetBool("moving", true);
	}


    void OnTriggerEnter(Collider coll) {
        switch (coll.gameObject.tag) {
            case "Bounds":
                //reverse direction
                distance += 0.5f;
                print("bounds encounter");
                if (horizontal == Direction.EAST)
                    horizontal = Direction.WEST;
                else if (horizontal == Direction.WEST)
                    horizontal = Direction.EAST;

                if (vertical == Direction.NORTH)
                    vertical = Direction.SOUTH;
                else if (vertical == Direction.SOUTH)
                    vertical = Direction.NORTH;
                break;
            case "Boomerang":
            case "PlayerProjectile":
                health -= 1;
                if (health <= 0)
                    Destroy(this.gameObject);
                break;
        }
    }


    public void change_dir() {
        int horiz = Mathf.FloorToInt(Random.Range(0, 3));
        int vert = Mathf.FloorToInt(Random.Range(0, 3));

        if (horiz == 0)
            horizontal = Direction.NORTH;
        else if (horiz == 1)
            horizontal = Direction.EAST;
        else
            horizontal = Direction.WEST;

        if (vert == 0)
            vertical = Direction.EAST;
        else if (vert == 1)
            vertical = Direction.NORTH;
        else
            vertical = Direction.SOUTH;
    }
}
