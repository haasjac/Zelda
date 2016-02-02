using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject rupee;
    public GameObject heart;
    public GameObject bomb;

    public GameObject cam;
    public float movement_speed = 2f;
    public float change_chance = 5;     //percent change to change direction
    public Direction current_dir = Direction.NORTH;
    public float target = 0f;
    public int health = 3;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //roll random path change     //make sure enemies can't go through doors!!!


    }

    public void FixedUpdate()
    {
        //if you're off camera, do nothing
        if (!Utils.on_camera(this.gameObject, cam))
            return;

        //if you've finished moving, choose a new direction and distance
        if (target <= 0.0f)
        {
            change_dir();
            target = Mathf.FloorToInt(Random.Range(1, 6));
            //if you will run into something, choose a new path
            if (!sonar(current_dir, this.gameObject, target))
                target = 0.0f;
        }
        else
        {

            //otherwise, move
            float move = Time.deltaTime;
            if (move > target)
                move = target;
            target -= move;

            Vector3 pos = this.transform.position;
            if (current_dir == Direction.NORTH)
                pos.y += move * movement_speed;
            else if (current_dir == Direction.SOUTH)
                pos.y -= move * movement_speed;
            else if (current_dir == Direction.EAST)
                pos.x += move * movement_speed;
            else
                pos.x -= move * movement_speed;

            this.transform.position = pos;
        }
    }

    public void change_dir()
    {
        int direction = Random.Range(0, 99);
        if (direction < 25)
            current_dir = Direction.NORTH;
        else if (direction < 50)
            current_dir = Direction.EAST;
        else if (direction < 75)
            current_dir = Direction.SOUTH;
        else if (direction < 100)
            current_dir = Direction.WEST;

    }

    void OnCollisionStay(Collision coll)
    {
        //if you collide with a solid object, change direction
        switch (coll.gameObject.tag)
        {
            case "Static":

            case "Locked":
                target = 1.0f;
                current_dir = change_direction(current_dir);
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Bounds":
                print("bounds encounter");
                target = 1.0f;
                current_dir = change_direction(current_dir);
                break;
            case "PlayerProjectile":
                health -= 1;
                if (health <= 0) {
                    Loot_drops.drop_item(rupee, heart, bomb, this.gameObject.transform.position);
                    Destroy(this.gameObject);
                }
                break;
            case "Static":

            case "Locked":
                target = 1.0f;
                current_dir = change_direction(current_dir);
                break;
            default:
                break;
        }
    }

    public Direction change_direction(Direction dir) {
        if (dir == Direction.NORTH)
            return Direction.EAST;
        else if (dir == Direction.EAST)
            return Direction.SOUTH;
        else if (dir == Direction.SOUTH)
            return Direction.WEST;
        else
            return Direction.NORTH;
    }

    public bool sonar(Direction dir, GameObject thing, float dist) {
        //return true if object is hit
        if (dir == Direction.NORTH) {
            Vector3 direction = new Vector3(0, 1, 0);
            return Physics.Raycast(thing.transform.position, direction, dist);
        }
        else if (dir == Direction.EAST) {
            Vector3 direction = new Vector3(1, 0, 0);
            return Physics.Raycast(thing.transform.position, direction, dist);
        }
        else if (dir == Direction.SOUTH) {
            Vector3 direction = new Vector3(0, -1, 0);
            return Physics.Raycast(thing.transform.position, direction, dist);
        }
        else {
            Vector3 direction = new Vector3(-1, 0, 0);
            return Physics.Raycast(thing.transform.position, direction, dist);
        }

    }
}