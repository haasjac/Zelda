using UnityEngine;
using System.Collections;

public class Boomerang : projectile {
    Vector3 target;
    Vector3 max_dis;
    Vector3 start;
    float startTime;
    float journeyLength;
    bool hit;

    void Start() {
        hit = false;
        PlayerControl.S.b_active = false;
        start = transform.position;
        target = start + direction * 4;
        max_dis = target;
        startTime = Time.time;
        journeyLength = Vector3.Distance(start, target);
    }

    void Update() {
        if (hit) {
            target = PlayerControl.S.gameObject.transform.position;
            journeyLength = Vector3.Distance(start, target);
        }
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(start, target, fracJourney);
        if (transform.position == max_dis) {
            start = transform.position;
            startTime = Time.time;
            hit = true;
        }
    }
    
    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player") {
            Destroy(this.gameObject);
            PlayerControl.S.b_active = true;
        }
        if (coll.gameObject.tag == "Bounds") {
            start = transform.position;
            startTime = Time.time;
            hit = true;
        }
        switch (coll.gameObject.tag) {
            case "Rupee":
                PlayerControl.S.rupee_count += 20;
                Destroy(coll.gameObject);
                break;
            case "Heart":
                if (PlayerControl.S.health + 1 > PlayerControl.S.max_health)
                    PlayerControl.S.health = PlayerControl.S.max_health;
                else
                    PlayerControl.S.health++;
                Destroy(coll.gameObject);
                break;
            case "Bomb":
                PlayerControl.S.bomb_count++;
                Destroy(coll.gameObject);
                break;
            case "Key":
                PlayerControl.S.key_count++;
                Destroy(coll.gameObject);
                break;
        }
        }
}
