using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StateHudPaused : State {
    Hud hd;
    int next_weapon;

    public StateHudPaused(Hud hd) {
        this.hd = hd;
    }

    public override void OnStart() {
        next_weapon = hd.curr_weapon;
    }

    public override void OnUpdate(float time_delta_fraction) {
        if (hd.has_weapon.Contains(true)) {
            float dir = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Horizontal")) {
                if (dir > 0) {
                    do {
                        if (next_weapon + 1 >= hd.weapons.Count)
                            next_weapon = 0;
                        else
                            next_weapon++;
                    } while (!hd.has_weapon[next_weapon]);
                } else {
                    do {
                        if (next_weapon == 0)
                            next_weapon = hd.weapons.Count - 1;
                        else
                            next_weapon--;
                    } while (!hd.has_weapon[next_weapon]);
                }
            }
            hd.weapons[hd.curr_weapon].GetComponentInChildren<Image>().color = Color.black;
            hd.curr_weapon = next_weapon;
            hd.weapons[hd.curr_weapon].GetComponentInChildren<Image>().color = Color.red;
        }
    }

    public override void OnFinish() {
        if (hd.has_weapon.Contains(true)) {
            switch (hd.curr_weapon) {
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
                    if (PlayerControl.S.has_red)
                        PlayerControl.S.selected_projectile_prefab = PlayerControl.S.cc_red_prefab;
                    else
                        PlayerControl.S.selected_projectile_prefab = PlayerControl.S.cc_blue_prefab;
                    break;
            }
        }
    }
}

public class StateHudUnpaused : State {
    Hud hd;

    public StateHudUnpaused(Hud hd) {
        this.hd = hd;
    }

    public override void OnStart() {
        if (hd.has_weapon.Contains(true)) {
            hd.b_button.sprite = PlayerControl.S.selected_projectile_prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

}

public class StateHudTrans : State {
    
    Hud hd;
    float target_y;
    float start_y;
    float startTime;
    float journeyLength;

    public StateHudTrans(Hud hd, float target_y) {
        this.hd = hd;
        this.target_y = target_y;
    }

    public override void OnStart() {
        this.start_y = hd.GetComponent<RectTransform>().anchoredPosition.y;
        startTime = Time.time;
        journeyLength = Mathf.Abs(start_y - target_y);
    }

    public override void OnUpdate(float time_delta_fraction) {
        if (hd.GetComponent<RectTransform>().anchoredPosition.y != target_y) {
            Vector3 pos = hd.GetComponent<RectTransform>().anchoredPosition;
            float distCovered = (Time.time - startTime) * hd.speed;
            float fracJourney = distCovered / journeyLength;
            pos.y = Mathf.Lerp(start_y, target_y, fracJourney);
            hd.GetComponent<RectTransform>().anchoredPosition = pos;
        } else {
            ConcludeState();
            if (hd.paused) {
                hd.hudMachine.ChangeState(new StateHudPaused(hd));
            } else {
                hd.hudMachine.ChangeState(new StateHudUnpaused(hd));
            }
        }
    }
}


