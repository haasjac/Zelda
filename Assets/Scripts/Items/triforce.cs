using UnityEngine;
using System.Collections;

public class triforce : MonoBehaviour {

    public string SceneName;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player") {
            Application.LoadLevel(SceneName);   
        }
    }
}
