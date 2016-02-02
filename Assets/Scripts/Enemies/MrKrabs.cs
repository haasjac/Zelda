using UnityEngine;
using System.Collections;

public class MrKrabs : MonoBehaviour {
    //TACTICS: -attack at every interval
    //         -slowly move back and forth (horizontally)

    //camera tracking
    public GameObject cam;

    //attack variables
    public GameObject link;
    public GameObject orb;
    public Vector3 target_middle;
    public Vector3 target_left;
    public Vector3 target_right;
    public float attack_cooldown = 2f;

    //color switch
    public bool blue = false;
    public float color_switch = 0.0f;

    //movement variables
    public int health = 10;
    public float distance = 2f;
    public float movement_speed = 1f;
    public float stun = 0.0f;                                   //can aquamentus be stunned?
    public Direction vertical = Direction.NORTH;

    // Use this for initialization
    void Start()
    {
        color_switch = 5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Utils.on_camera(this.gameObject, cam))
            return;

        if (stun > 0.0f)
        {
            stun -= Time.deltaTime;
            return;
        }

        //is it time to switch?
        if(color_switch <= 0.0f)
        {
            if (blue) {
                GetComponent<Animator>().SetBool("blue", false);
                blue = false;
            }
            else
            {
                GetComponent<Animator>().SetBool("blue", true);
                blue = true;
            }
            color_switch = 5f;
        }
        else
        {
            color_switch -= Time.deltaTime;
        }

        //direction update
        if (distance <= 0.0f)
        {
            distance = 2f;
            if (vertical == Direction.SOUTH)
                vertical = Direction.NORTH;
            else
                vertical = Direction.SOUTH;
        }

        //attack and movement updates
        if (attack_cooldown <= 0.0f)
        {
            attack();
            attack_cooldown = 2.5f;
        }
        else
        {
            Vector3 pos = this.transform.position;
            float shift = Time.deltaTime * movement_speed;
            //move and decrease cooldown
            if (vertical == Direction.NORTH)
                pos.y += shift;
            else if (vertical == Direction.SOUTH)
                pos.y -= shift;

            distance -= shift;
            attack_cooldown -= Time.deltaTime;
            this.transform.position = pos;
        }
    }


    void attack()
    {
        print("orbs away!");
        Vector3 link_pos = link.transform.position;
        //instantiate 3 orbs and hurl them at the player
        //extend x value to ensure each orb collides with something
        if (link_pos.x < transform.position.x)
            link_pos.x -= 200f;
        else
            link_pos.x += 200f;


        //instance location
        Vector3 pos = transform.position;
        pos.x -= 0.75f;
        pos.y += 0.75f;
        GameObject middle = Instantiate<GameObject>(orb);               //orbs are triggers, MUST add OnTrigger damage
        middle.transform.position = pos;
        middle.GetComponent<orb_projectile>().target = link_pos;
    }


    void OnTriggerEnter(Collider coll)
    {
        switch (coll.gameObject.tag)
        {
            case "PlayerProjectile":
                //check what color link is vs. what color you are

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