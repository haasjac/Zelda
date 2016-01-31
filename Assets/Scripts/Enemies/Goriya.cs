using UnityEngine;
using System.Collections;

public class Goriya : Gel {

    public GameObject boomerang;
    public float attack_cooldown = 0.0f;

    // Use this for initialization
    void Start() {
        target_cooldown = Random.Range(0.5f, 3.0f);
 //       InvokeRepeating("AnimationUpdate", 0f, 0.1f);
    }

    // Update is called once per frame
    void Update() {

        attack_cooldown -= Time.deltaTime;

        AnimationUpdate();
        if (rest && attack_cooldown <= 0.0f) {
            attack();
        }
    }

    void AnimationUpdate() {
        if (direction == Direction.NORTH)
            GetComponent<Animator>().SetInteger("direction", 0);
        else if(direction == Direction.EAST)
            GetComponent<Animator>().SetInteger("direction", 1);
        else if (direction == Direction.SOUTH)
            GetComponent<Animator>().SetInteger("direction", 2);
        else if (direction == Direction.WEST)
            GetComponent<Animator>().SetInteger("direction", 3);

    }

    void attack() {
        Vector3 pos = transform.position;
        if (direction == Direction.NORTH)
            pos.y += 1;
        else if (direction == Direction.EAST)
            pos.x += 1;
        else if (direction == Direction.SOUTH)
            pos.y -= 1;
        else if (direction == Direction.WEST)
            pos.x -= 1;

        //instantiate boomerang and hurl in current direction
        GameObject batarang = Instantiate<GameObject>(boomerang);       //debug boomerang behaviour
        batarang.GetComponent<goriya_boomer>().direction = direction;
        batarang.transform.position = pos;

        //rotate boomerang appropriately
        if(direction == Direction.NORTH)
            batarang.transform.Rotate(Vector3.forward * -90);
        else if (direction == Direction.SOUTH)
            batarang.transform.Rotate(Vector3.forward * -270);
        else if (direction == Direction.EAST)
            batarang.transform.Rotate(Vector3.forward * -180);

        attack_cooldown = 5.0f;
    }
}
