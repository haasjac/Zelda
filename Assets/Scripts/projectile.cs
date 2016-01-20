using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour {

    public float speed = 0.1f;
    public Vector3 direction;
    public string button;

	// Use this for initialization
	void Start () {
        if (button == "a") {
            PlayerControl.S.a_active = false;
        }
        if (button == "b") {
            PlayerControl.S.b_active = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += direction * speed;
	}

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Bounds") {
            Destroy(this.gameObject);
            if (button == "a") {
                PlayerControl.S.a_active = true;
            }
            if (button == "b") {
                PlayerControl.S.b_active = true;
            }
        }
    }
}


