using UnityEngine;
using System.Collections;

public class orb_projectile : MonoBehaviour {

    public Vector3 target;
    public float movement_speed = 5f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {                               //ALTER: orb path - stagger vertical shift
        Vector3 pos = this.transform.position;
        float shift = Time.deltaTime * movement_speed;

        if (target.x > pos.x)
            pos.x += shift;
        else if (target.x < pos.x)
            pos.x -= shift;

        if (target.y > pos.y)
            pos.y += shift;
        else if (target.y < pos.y)
            pos.y -= shift;

        this.transform.position = pos;
    }


    void OnTriggerEnter(Collider coll) {
        switch (coll.gameObject.tag) {
            case "Enemy":
                break;
            default:
                Destroy(this.gameObject);
                break;
        }
    }
}
