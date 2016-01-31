using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pickup : MonoBehaviour {

    public int list_num;
    public Image cc = null;
    public Image b_field = null;
    public bool red = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player") {
            Hud.S.has_weapon[list_num] = true;
            Hud.S.weapons[list_num].SetActive(true);
            Destroy(this.gameObject);
            if (red) {
                cc.sprite = PlayerControl.S.cc_red_prefab.GetComponent<SpriteRenderer>().sprite;
                PlayerControl.S.has_red = true;
                if (PlayerControl.S.selected_projectile_prefab == PlayerControl.S.cc_blue_prefab) {
                    PlayerControl.S.selected_projectile_prefab = PlayerControl.S.cc_red_prefab;
                    b_field.sprite = PlayerControl.S.cc_red_prefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
            if (PlayerControl.S.selected_projectile_prefab == null && PlayerControl.S.selected_weapon_prefab == null) {
                switch (list_num) {
                    case 0:
                        PlayerControl.S.selected_weapon_prefab = null;
                        PlayerControl.S.selected_projectile_prefab = PlayerControl.S.bomb_prefab;
                        break;
                    case 1:
                        PlayerControl.S.selected_weapon_prefab = null;
                        PlayerControl.S.selected_projectile_prefab = PlayerControl.S.boomerang_prefab;
                        break;
                    case 2:
                        PlayerControl.S.selected_weapon_prefab = PlayerControl.S.bow_prefab;
                        PlayerControl.S.selected_projectile_prefab = PlayerControl.S.arrow_prefab;
                        break;
                    case 3:
                        PlayerControl.S.selected_weapon_prefab = null;
                        if (!red)
                            PlayerControl.S.selected_projectile_prefab = PlayerControl.S.cc_blue_prefab;
                        else
                            PlayerControl.S.selected_projectile_prefab = PlayerControl.S.cc_red_prefab;
                        break;
                }
                Hud.S.curr_weapon = list_num;
                Hud.S.b_button.sprite = PlayerControl.S.selected_projectile_prefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}
