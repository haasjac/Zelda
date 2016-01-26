using UnityEngine;
using System.Collections;

public class bomb : projectile {


    public GameObject explosion;
    public float time_alive = 4f;
    float timer;

    // Use this for initialization
    void Start() {
        PlayerControl.S.b_active = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        if (timer >= time_alive) {
            GameObject exp = Instantiate<GameObject>(explosion);
            exp.transform.position = this.transform.position;
            Destroy(this.gameObject);
            PlayerControl.S.b_active = true;
        }
    }

    void OnTriggerEnter(Collider coll) {
        
    }
}