using UnityEngine;
using System.Collections;

public class Gel : MonoBehaviour {

    public GameObject cam;

    public float movement_speed = 1f;
    public float distance = 0.0f;
    public float cooldown = 0.0f;
    public float target_cooldown = 0.0f;
    public int health = 1;
    public bool rest = false;
    public Direction direction = Direction.NORTH;

	// Use this for initialization
	void Start () {
        target_cooldown = Random.Range(0.5f, 3.0f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (!rest) {
            cooldown += Time.deltaTime;
            if (cooldown >= target_cooldown) {
                rest = true;
                cooldown = 1.5f;
            }
        }

        if (rest && distance <= 0.0f) {                //if resting, don't move
            cooldown -= Time.deltaTime;
//            change_direction();
            if (cooldown <= 0.0f) { 
                rest = false;
                change_direction();
            }
        }
        else if (!check_direction(this.gameObject, cam) && distance <= 0.0f) {
            change_direction();
        }
        else if(distance <= 0.0f) {
            //choose a new direction
            distance = 1f;
            //            while (!check_direction(this.gameObject, cam))
//            change_direction();
        }
        else {  //move
            Vector3 pos = this.transform.position;
            float shift = Time.deltaTime * movement_speed;
            if (shift > distance)
                shift = distance;

            if (direction == Direction.NORTH)
                pos.y += shift;
            else if (direction == Direction.SOUTH)
                pos.y -= shift;
            else if (direction == Direction.EAST)
                pos.x += shift;
            else
                pos.x -= shift;

            distance -= shift;
            this.transform.position = pos;
        }
	}


    public void change_direction() {
        int dir = Mathf.FloorToInt(Random.Range(0, 4));
        if (dir == 0)
            direction = Direction.NORTH;
        else if (dir == 1)
            direction = Direction.SOUTH;
        else if (dir == 2)
            direction = Direction.EAST;
        else
            direction = Direction.WEST;
    }


    public bool check_direction(GameObject thing, GameObject cam) { //h = 5.5, v = 3
        Vector3 pos = this.transform.position;
        Vector3 cam_pos = cam.transform.position;
        
        if (direction == Direction.EAST && pos.x + 1 >= cam_pos.x + 5.6)
            return false;
        if (direction == Direction.WEST && pos.x - 1 <= cam_pos.x - 5.6)
            return false;
        if (direction == Direction.NORTH && pos.y + 1 >= cam_pos.y + 1.6)
            return false;
        if (direction == Direction.SOUTH && pos.y - 1 <= cam_pos.y - 4.6)
            return false;

        return true;
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
