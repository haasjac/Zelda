using UnityEngine;
using System.Collections;

public class item : MonoBehaviour {

    public float time_alive = 4f;
    float timer;

	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= time_alive) {
            Destroy(this.gameObject);
        }
	}
}
