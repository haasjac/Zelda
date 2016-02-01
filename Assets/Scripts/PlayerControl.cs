using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

[System.Serializable]
public class linkSprite {
    public Sprite[] link_run_down;
    public Sprite[] link_run_up;
    public Sprite[] link_run_right;
    public Sprite[] link_run_left;
}

public class PlayerControl : MonoBehaviour {
    
    public static PlayerControl S;

    public float walking_veloctiy = 1.0f;

    public int rupee_count = 0;
    public int key_count = 0;
    public int bomb_count = 0;
    public float health = 3.0f;
    public float max_health = 3.0f;

    public bool invincible = false;
    
    public linkSprite green;
    public linkSprite blue;
    public linkSprite red;
    [HideInInspector]
    public linkSprite link_sprite;


    StateMachine animation_state_machine;
	public StateMachine control_state_machine;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;

	public GameObject selected_weapon_prefab;
    public GameObject selected_projectile_prefab;
    public GameObject wooden_sword_prefab;
    public GameObject white_sword_prefab;
    public GameObject boomerang_prefab;
    public GameObject bow_prefab;
    public GameObject arrow_prefab;
    public GameObject bomb_prefab;
    public GameObject cc_blue_prefab;
    public GameObject cc_red_prefab;
    public bool has_red = false;

    public GameObject room_view;
    public GameObject stagger_block;
    public Direction stagger_dir = Direction.EAST;
    public Direction cam_dir = Direction.EAST;
    public float push_cooldown = 0;
    public float cam_shift = 0;
    public Vector3 link_hold = Vector3.zero;
    public float link_stun = 0.0f;

//    public Sprite sprite_tl;
    public Sprite sprite_t;
    public Sprite sprite_l;
    public Sprite sprite_r;

    public GameObject map_ref;

    public Image fade;
    public float fade_speed = 1;

    [HideInInspector]
    public bool a_active = true;
    [HideInInspector]
    public bool b_active = true;

    [HideInInspector]
    public float dead_timer;
    [HideInInspector]
    public Vector3 start_pos;
    [HideInInspector]
    public Vector3 start_pos_cam;

    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 60;

        if(S != null) {
            Debug.LogError("WTF TWO LINKS???");
        }
        S = this;

        link_sprite = green;
        dead_timer = 0;
        start_pos = transform.position;
        start_pos_cam = room_view.transform.position;

        animation_state_machine = new StateMachine();
        animation_state_machine.ChangeState(new StateIdleWithSprite(this, GetComponent<SpriteRenderer>(), link_sprite.link_run_down[0]));
        control_state_machine = new StateMachine();
        control_state_machine.ChangeState(new StateLinkNormalMovement(this));
	}

    // Update is called once per frame
    void Update() {

        if (health <= 0) {
            //this.gameObject.SetActive(false);
            control_state_machine.ChangeState(new StateLinkStunned(this));
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            dead_timer += Time.deltaTime;
            if (dead_timer > 2) {
                transform.position = start_pos;
                room_view.transform.position = start_pos_cam;
                health = max_health;
                dead_timer = 0;
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                control_state_machine.ChangeState(new StateLinkNormalMovement(this));
            }
            //Destroy(this.gameObject);
        }


        animation_state_machine.Update();

        control_state_machine.Update();
        if (control_state_machine.IsFinished()) {
            control_state_machine.ChangeState(new StateLinkNormalMovement(this));
        }
    }

    void FixedUpdate() {
        if (push_cooldown > 0.0f) {      //move the block
            push_cooldown -= Time.deltaTime;

            Vector3 pos = stagger_block.transform.position;
            if (stagger_dir == Direction.EAST)
                pos.x += 0.02f;
            else if (stagger_dir == Direction.WEST)
                pos.x -= 0.02f;
            else if (stagger_dir == Direction.NORTH)
                pos.y += 0.02f;
            else if (stagger_dir == Direction.SOUTH)
                pos.y -= 0.02f;

            stagger_block.transform.position = pos;
        } else if (cam_shift > 0) {   //move the camera
            Vector3 pos = room_view.transform.position;
            float shift = Time.deltaTime * 10;
            if (shift > cam_shift)
                shift = cam_shift;
            if (cam_dir == Direction.EAST)
                pos.x += shift;
            else if (cam_dir == Direction.WEST)
                pos.x -= shift;
            else if (cam_dir == Direction.NORTH)
                pos.y += shift;
            else if (cam_dir == Direction.SOUTH)
                pos.y -= shift;

            cam_shift -= shift;
            room_view.transform.position = pos;

            //keep player still
            transform.position = link_hold;
        }
        else if(link_stun > 0.0f) {
            link_stun -= Time.deltaTime;
            transform.position = link_hold;
        }
    }

    void OnTriggerEnter(Collider coll) {
        
        switch (coll.gameObject.tag) {
            case "Rupee":
                rupee_count += 20;
                Destroy(coll.gameObject);
                break;
            case "Heart":
                if (health + 1 > max_health)
                    health = max_health;
                else
                    health++;
                Destroy(coll.gameObject);
                break;
            case "Bomb":
                bomb_count += 4;
                Destroy(coll.gameObject);
                break;
            case "Bomb_pickup":
                bomb_count += 4;
                Destroy(coll.gameObject);
                break;
            case "Key":
                key_count++;
                Destroy(coll.gameObject);
                break;
            case "Enemy":   //for the sake of debugging blade traps
                //if (!invincible)
                //    health -= 0.5f;
                //break;
            case "EnemyProjectile":
                if (!invincible)
                    health -= 0.5f;
                break;
            case "Enemy_boomerang":
                link_hold = transform.position;
                link_stun = 2.0f;
                break;
            case "Door":
                if (cam_shift > 0.0f)
                    break;
                Vector3 link_pos = this.gameObject.transform.position;
                //horizontal shift = 16 units
                //vertical shift = 11 units
                if (current_direction == Direction.EAST) {
                    link_pos.x += 3f;
                    cam_shift = 16;
                    cam_dir = Direction.EAST;
                } else if (current_direction == Direction.WEST) {
                    link_pos.x -= 3f;
                    cam_shift = 16;
                    cam_dir = Direction.WEST;
                } else if (current_direction == Direction.NORTH) {
                    link_pos.y += 3f;
                    cam_shift = 11;
                    cam_dir = Direction.NORTH;
                } else if (current_direction == Direction.SOUTH) {
                    link_pos.y -= 3f;
                    cam_shift = 11;
                    cam_dir = Direction.SOUTH;
                }

                S.transform.position = link_pos;
                link_hold = link_pos;
                break;
            case "Stairs":
                control_state_machine.ChangeState(new StateLinkStunned(this));
                /*for(float i = 0; i < fade_speed; i += Time.deltaTime) {
                    print(i);
                    Color temp = fade.color;
                    temp.a = (i / fade_speed) * 255;
                    fade.color = temp;
                }*/
                transform.position = new Vector3(19, 53, 0);
                room_view.transform.position = new Vector3(23.5f, 50.5f, -10);
                /*for (float i = 0; i < fade_speed; i += Time.deltaTime) {
                    Color temp = fade.color;
                    temp.a = 255 - (i / fade_speed) * 255;
                    fade.color = temp;
                }
                Color c = fade.color;
                c.a = 0;
                fade.color = c;*/
                control_state_machine.ChangeState(new StateLinkNormalMovement(this));
                break;
            case "ExitBR":
                control_state_machine.ChangeState(new StateLinkStunned(this));

                transform.position = new Vector3(21, 59, 0);
                room_view.transform.position = new Vector3(23.5f, 61.5f, -10);
                control_state_machine.ChangeState(new StateLinkNormalMovement(this));
                break;
        }
    }

    void OnCollisionEnter(Collision coll) {

        switch (coll.gameObject.tag) {
            case "Water":
                if (link_sprite == blue) {
                    coll.collider.isTrigger = true;
                }
                break;
            case "Lava":
                if (link_sprite == red) {
                    coll.collider.isTrigger = true;
                }
                break;
            case "Enemy":
                control_state_machine.ChangeState(new StateDamaged(this, coll.contacts[0].normal, 0.5f));
                break;
            case "EnemyProjectile":
                bool shield = false;
                if (GetComponent<Rigidbody>().velocity == Vector3.zero) {
                    if (coll.contacts[0].normal == Vector3.up && current_direction == Direction.NORTH)
                        shield = true;
                    if (coll.contacts[0].normal == Vector3.right && current_direction == Direction.EAST)
                        shield = true;
                    if (coll.contacts[0].normal == Vector3.down && current_direction == Direction.SOUTH)
                        shield = true;
                    if (coll.contacts[0].normal == Vector3.left && current_direction == Direction.WEST)
                        shield = true;
                }
                if (!shield)
                    control_state_machine.ChangeState(new StateDamaged(this, coll.contacts[0].normal, 0.5f));
                break;
            case "Pushable":
                if (this.gameObject.tag == "Player") {
                    print("Pushable Collision");
                    push_cooldown = 1.0f;
                    stagger_block = coll.gameObject;
                    stagger_dir = current_direction;
                    //each "pushable" block may only be moved once
                    coll.gameObject.tag = "Static";
                }
                break;
            case "Locked":
                print("Do you have the POWER???");
                if (key_count > 0) {
                    //make this fancy! (maybe add animations? spawn a replacement tile?)
                    //80, 81, 101, 106
                    //                    Vector3 pos = coll.gameObject.transform.position;
                    //BoxCollider tile_coll = coll.gameObject.GetComponent<Tile>().GetComponent<BoxCollider>();
                    //if (coll.gameObject.GetComponent<Tile>().tileNum == 80) {    //upper-left
                    //    //no more solid collisions
                    //    tile_coll.center = new Vector3(0, 3, 0);
                    //    tile_coll.size = Vector3.zero;
                    //} else if (coll.gameObject.GetComponent<Tile>().tileNum == 81) {    //upper-right
                    //    tile_coll.center = new Vector3(-0.5f, 0.33f, 0);
                    //    tile_coll.size = new Vector3(1.75f, 0.5f, 1);
                    //} else if (coll.gameObject.GetComponent<Tile>().tileNum == 101) {    //right
                    //    tile_coll.center = new Vector3(0.5f, 0, 0);
                    //    tile_coll.size = new Vector3(0.5f, 1, 1);
                    //} else if (coll.gameObject.GetComponent<Tile>().tileNum == 106) {    //left
                    //    tile_coll.center = new Vector3(-0.5f, 0, 0);
                    //    tile_coll.size = new Vector3(-0.5f, 1, 1);
                    //}
                    BoxCollider locked = coll.gameObject.GetComponent<BoxCollider>();
/*                   if (coll.gameObject.name == "locked_TL")
                    {    //upper-left
                        //no more solid collisions
                        locked.center = new Vector3(0, 3 / 6f, 0);
                        locked.size = Vector3.zero;
                        coll.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_tl;
                    }
                    else*/  if (coll.gameObject.name == "locked_T")
                    {    //upper-right
                        locked.center = new Vector3(0, 0.8f, 0);
                        locked.size = new Vector3(1.75f, 0.5f, 1);
                        coll.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_t;
                    }
                    else if (coll.gameObject.name == "locked_R")
                    {    //right
                        locked.center = new Vector3(0.5f, 0, 0);
                        locked.size = new Vector3(0.5f, 1, 1);
                        coll.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_r;
                    }
                    else if (coll.gameObject.name == "locked_L")
                    {    //left
                        locked.center = new Vector3(-0.5f, 0, 0);
                        locked.size = new Vector3(-0.5f, 1, 1);
                        coll.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_l;
                    }

                    locked.isTrigger = true;
                    coll.gameObject.tag = "Door";
                    
                }
                break;
        }
    }

    void OnCollisionStay(Collision coll) {
        switch (coll.gameObject.tag) {
            case "Pushable":
                break;
        }
    }


    void OnTriggerExit(Collider coll) {
        switch (coll.gameObject.tag) {
            case "Water":
                coll.isTrigger = false;
                break;
            case "Lava":
                coll.isTrigger = false;
                break;
        }
    }

    public void pcInvoke(string func, float time) {
        Invoke(func, time);
    }

    public void makeVulnerable() {
        invincible = false;
    }
}
