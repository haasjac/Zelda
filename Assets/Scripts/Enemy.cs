using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float movement_speed = 3f;
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
        if (target <= 0.0f)
        {
            change_dir();
            target = Random.Range(0, 3);
        }

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
            case "Solid":

            case "Locked":
                target = 0.0f;
                if (current_dir == Direction.NORTH)
                    current_dir = Direction.SOUTH;
                else if (current_dir == Direction.SOUTH)
                    current_dir = Direction.NORTH;
                else if (current_dir == Direction.EAST)
                    current_dir = Direction.WEST;
                else
                    current_dir = Direction.EAST;
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
                if (current_dir == Direction.NORTH)
                    current_dir = Direction.SOUTH;
                else if (current_dir == Direction.SOUTH)
                    current_dir = Direction.NORTH;
                else if (current_dir == Direction.EAST)
                    current_dir = Direction.WEST;
                else
                    current_dir = Direction.EAST;
                break;
            case "PlayerProjectile":
                health -= 1;
                if (health <= 0)
                    Destroy(this.gameObject);
                break;
        }
    }
}