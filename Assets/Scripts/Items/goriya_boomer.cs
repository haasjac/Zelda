using UnityEngine;
using System.Collections;

public class goriya_boomer : MonoBehaviour {

    public float dest = 4f;
    public float current = 0.0f;
    public float movement_speed = 1f;
    public bool back = false;
    public Direction direction = Direction.NORTH;

	// Use this for initialization
	void Start () {
        current = dest;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (current <= 0.0f && back == true)
            Destroy(this.gameObject);
        if (current <= 0.0f && back == false) { 
            direction = reverse(direction);
            back = true;
            current = dest;
        }

        float shift = Time.deltaTime * movement_speed;
        Vector3 pos = this.transform.position;
        if (direction == Direction.NORTH)
            pos.y += shift;
        else if (direction == Direction.SOUTH)
            pos.y -= shift;
        else if (direction == Direction.EAST)
            pos.x += shift;
        else
            pos.x -= shift;

        current -= shift;
        this.transform.position = pos;

    }

    Direction reverse(Direction dir) {
        if (dir == Direction.NORTH)
            return Direction.SOUTH;
        else if (dir == Direction.SOUTH)
            return Direction.NORTH;
        else if (dir == Direction.EAST)
            return Direction.WEST;
        else
            return Direction.EAST;
    }

    void OnTriggerEnter(Collider coll) {        //Static encounters not being triggered
        switch (coll.gameObject.tag) {
            case "Static":
                dest -= current;
                current = 0.0f;
                back = true;
                direction = reverse(direction);
                break;
        }
    }

}
