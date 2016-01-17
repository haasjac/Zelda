using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerControl : MonoBehaviour {
    
    public static PlayerControl S;

    public float walking_veloctiy = 1.0f;

    public int rupee_count = 0;
    public int key_count = 0;
    public int bomb_count = 0;
    public float health = 3.0f;

    public bool invincible = false;

    public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;

	StateMachine animation_state_machine;
	StateMachine control_state_machine;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;

	public GameObject selected_weapon_prefab;

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 60;

        if(S != null) {
            Debug.LogError("WTF TWO LINKS???");
        }
        S = this;

        animation_state_machine = new StateMachine();
        animation_state_machine.ChangeState(new StateIdleWithSprite(this, GetComponent<SpriteRenderer>(), link_run_down[0]));
        control_state_machine = new StateMachine();
        control_state_machine.ChangeState(new StateLinkNormalMovement(this));
	}

    // Update is called once per frame
    void Update() {

        if (health <= 0)
            Destroy(this.gameObject);

        animation_state_machine.Update();

        control_state_machine.Update();
        if(control_state_machine.IsFinished()) {
            control_state_machine.ChangeState(new StateLinkNormalMovement(this));
        }
    }

    void OnTriggerEnter(Collider coll) {

        switch (coll.gameObject.tag) {
            case "Rupee":
                rupee_count++;
                Destroy(coll.gameObject);
                break;
            case "Heart":
                health++;
                Destroy(coll.gameObject);
                break;
            case "Bomb":
                bomb_count++;
                Destroy(coll.gameObject);
                break;
            case "Key":
                key_count++;
                Destroy(coll.gameObject);
                break;
        }
    }

    void OnCollisionEnter(Collision coll) {

        switch (coll.gameObject.tag) {
            case "Enemy":
                control_state_machine.ChangeState(new StateDamaged(this, coll.contacts[0].normal, 0.5f));
                break;
            case "EnemyProjectile":
                control_state_machine.ChangeState(new StateDamaged(this, coll.contacts[0].normal, 0.5f));
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
