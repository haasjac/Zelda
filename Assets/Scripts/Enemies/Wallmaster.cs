using UnityEngine;
using System.Collections;

public class Wallmaster : MonoBehaviour {

    public Vector3 origin;
    public GameObject cam;
    public GameObject link;
    public float x = 39.5f;
    public float y = 6.5f;
    public float movement_speed = 1f;
    public int health = 3;
    public float flee_cooldown = 0.0f;
    public bool flee = false;
    public Direction direction = Direction.NORTH;

    // Use this for initialization
    void Start () {
        //staggered arrival for multiple wallmasters
        flee_cooldown = Random.Range(0.5f, 3f);
        origin = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        //is your room on screen?
        //PERFORM CHECK HERE
        if (visibility_check(this.gameObject, cam)) {
            //move closer to player
            Vector3 link_pos = link.transform.position;
            Vector3 pos = this.transform.position;

            float shift = Time.deltaTime * movement_speed;
            if (!flee) { 
                //prioritize vertical movement over horizontal
                if (link_pos.y > pos.y)
                    pos.y += shift;
                else if (link_pos.y < pos.y)
                    pos.y -= shift;

                if (link_pos.x > pos.x)
                    pos.x += shift;
                else if (link_pos.x < pos.x)
                    pos.x -= shift;
            }
            else {
                pos.y -= shift;
                flee_cooldown -= shift;
                if (flee_cooldown <= 0.0f)
                    flee = false;
            }

            this.transform.position = pos;
        }
        else {
            //move back to origin
            this.transform.position = origin;
        }
    }

    void OnCollisionEnter(Collision coll) {
        switch (coll.gameObject.tag) {
            case "Player":
                //move link and camera back to start
                Vector3 pos = new Vector3(x, y, -10);
                cam.transform.position = pos;
                pos.z = 0;
                link.transform.position = pos;
                break;
            case "PlayerProjectile":
                health -= 1;
                if (health <= 0)
                    Destroy(this.gameObject);
                else
                    Flee();
                break;
        }
    }


    public void Flee() {
        flee = true;
        //move back towards south wall
        flee_cooldown = Random.Range(0.5f, 3f);
    }

    public bool visibility_check(GameObject thing, GameObject cam) { //h = 5.5, v = 3
        Vector3 pos = this.transform.position;
        Vector3 cam_pos = cam.transform.position;

        if (pos.x >= cam_pos.x + 6.5)
            return false;
        if (pos.x <= cam_pos.x - 6.5)
            return false;
        if (pos.y >= cam_pos.y + 4.0)
            return false;
        if (pos.y <= cam_pos.y - 5.0)
            return false;

        return true;
    }
}
