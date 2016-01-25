using UnityEngine;
using System.Collections;

public class Blade_trap : MonoBehaviour {

    public GameObject link;
    public GameObject cam;
    public Direction direction = Direction.NORTH;
    public float horizontal = 5.0f;
    public float vertical = 2.5f;

    public float dist_forward = 0.0f;
    public float dist_back = 0.0f;
    public float movement_speed = 10f;
    public bool move_back = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {
        Vector3 link_pos = link.transform.position;
        Vector3 pos = this.transform.position;
        
        if(withinBounds(this.gameObject, cam)) {    //object must be withing camera bounds
            if (dist_back <= 0.0f && dist_forward <= 0.0f) {
                    //track link relative to blade position
                    move_back = false;
                    if (Mathf.Round(link_pos.x) == Mathf.Round(pos.x)) {            //vertical movement
                    dist_back = dist_forward = vertical;
                        if (link_pos.y > pos.y) 
                            direction = Direction.NORTH;
                        else 
                            direction = Direction.SOUTH;
                    }
                    else if (Mathf.Round(link_pos.y) == Mathf.Round(pos.y)) {       //horizontal movement
                        dist_back = dist_forward = horizontal;
                        if (link_pos.x > pos.x)
                            direction = Direction.EAST;
                        else 
                            direction = Direction.WEST;
                    }
                }
        
            else {  //move the object
                float shift = Time.deltaTime * movement_speed;
                if (!move_back) {
                    if (shift >= dist_forward) {
                        shift = dist_forward;
                        dist_forward = 0.0f;
                        move_back = true;
                    }
                    else
                        dist_forward -= shift;
                }
                else {
                    shift /= 3;
                    if (shift >= dist_back) {
                        shift = dist_back;
                        dist_back = 0.0f;
                    }
                    else
                        dist_back -= shift;
                }

                if (direction == Direction.NORTH)
                    pos.y += shift;
                else if (direction == Direction.SOUTH)
                    pos.y -= shift;
                else if (direction == Direction.EAST)
                    pos.x += shift;
                else
                    pos.x -= shift;

                if (move_back && (dist_back == 2.5f || dist_back == 5.0f))  //should only trigger once per attack
                    direction = reverse(direction);
                this.transform.position = pos;
            }
        }
    }

    static public Direction reverse(Direction dir) {
        if (dir == Direction.NORTH)
            dir = Direction.SOUTH;
        else if (dir == Direction.SOUTH)
            dir = Direction.NORTH;
        else if (dir == Direction.EAST)
            dir = Direction.WEST;
        else
            dir = Direction.EAST;

        return dir;
    }

    static public bool withinBounds(GameObject thing, GameObject cam) {
        Vector3 pos = thing.transform.position;
        Vector3 cam_pos = cam.transform.position;

        if (pos.x > cam_pos.x + 7)
            return false;
        if (pos.x < cam_pos.x - 7)
            return false;
        if (pos.y > cam_pos.y + 5)
            return false;
        if (pos.y < cam_pos.y - 5)
            return false;

        return true;
    }
}
