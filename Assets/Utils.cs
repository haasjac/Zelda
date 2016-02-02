using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

    public static bool on_camera(GameObject thing, GameObject cam) {
        Vector3 pos = thing.transform.position;
        Vector3 cam_pos = cam.transform.position;

        if (pos.x >= cam_pos.x + 8.6)
            return false;
        if (pos.x <= cam_pos.x - 8.6)
            return false;
        if (pos.y >= cam_pos.y + 6.6)
            return false;
        if (pos.y <= cam_pos.y - 6.6)
            return false;

        return true;
    }

    public static bool check_movement(Direction dir, GameObject thing, GameObject cam)
    { //h = 5.5, v = 3
        Vector3 pos = thing.transform.position;
        Vector3 cam_pos = cam.transform.position;

        if (!on_camera(thing, cam))
            return false;
        if (dir == Direction.EAST && pos.x + 1 >= cam_pos.x + 6.6)
            return false;
        if (dir == Direction.WEST && pos.x - 1 <= cam_pos.x - 6.6)
            return false;
        if (dir == Direction.NORTH && pos.y + 1 >= cam_pos.y + 2.6)
            return false;
        if (dir == Direction.SOUTH && pos.y - 1 <= cam_pos.y - 4.6)
            return false;

        return true;
    }

    public void damage_color(GameObject enemy)
    {
        enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
        enemy.GetComponent<Renderer>().material.color = Color.white;
    }

    //public void check_movement(GameObject thing, Direction dir)
    //{
    //    Vector3 pos = thing.transform.position;
    //    int x = Mathf.FloorToInt(pos.x);
    //    int y = Mathf.FloorToInt(pos.y);

    //    if (dir == Direction.NORTH)
    //        y += 1;
    //    else if (dir == Direction.SOUTH)
    //        y -= 1;
    //    else if (dir == Direction.EAST)
    //        x += 1;
    //    else
    //        x -= 1;

    //    //check the tile
    //    return (ShowMapOnCamera.MAP_TILES[x, y].gameObject.GetComponent<Tile>().sprend.sprite == spriteArray[29]);

    //}
}
