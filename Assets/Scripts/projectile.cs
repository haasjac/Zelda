using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour {

    public float speed = 0.1f;
    public Vector3 direction;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += direction * speed;
	}

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Bounds") {
            Destroy(this.gameObject);
        }
    }
}
