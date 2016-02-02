using UnityEngine;
using System.Collections;

public class Loot_drops : MonoBehaviour {

    public static void drop_item(GameObject rupee, GameObject heart, GameObject bomb, Vector3 pos) {
        int item = Mathf.FloorToInt(Random.Range(0, 100));
        GameObject spawn;
        //nothing
        if (item < 60)
            return;
        //rupee
        else if(item < 80)
            spawn = Instantiate<GameObject>(rupee);
        //heart
        else if (item < 90)
            spawn = Instantiate<GameObject>(heart);
        //bomb
        else
            spawn = Instantiate<GameObject>(bomb);

        spawn.transform.position = pos;
        return;
    }
}
