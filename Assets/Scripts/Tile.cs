using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    static Sprite[]         spriteArray;

    public Texture2D        spriteTexture;
	public int				x, y;
	public int				tileNum;
	private BoxCollider		bc;
    private Material        mat;

    private SpriteRenderer  sprend;

	void Awake() {
        if (spriteArray == null) {
            spriteArray = Resources.LoadAll<Sprite>(spriteTexture.name);
        }

		bc = GetComponent<BoxCollider>();

        sprend = GetComponent<SpriteRenderer>();
        //Renderer rend = gameObject.GetComponent<Renderer>();
        //mat = rend.material;
	}

	public void SetTile(int eX, int eY, int eTileNum = -1) {
		if (x == eX && y == eY) return; // Don't move this if you don't have to. - JB

		x = eX;
		y = eY;
		transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3")+"x"+y.ToString("D3");

		tileNum = eTileNum;
		if (tileNum == -1 && ShowMapOnCamera.S != null) {
			tileNum = ShowMapOnCamera.MAP[x,y];
			if (tileNum == 0) {
				ShowMapOnCamera.PushTile(this);
			}
		}

        sprend.sprite = spriteArray[tileNum];

		if (ShowMapOnCamera.S != null) SetCollider();
        //TODO: Add something for destructibility - JB

        gameObject.SetActive(true);
		if (ShowMapOnCamera.S != null) {
			if (ShowMapOnCamera.MAP_TILES[x,y] != null) {
				if (ShowMapOnCamera.MAP_TILES[x,y] != this) {
					ShowMapOnCamera.PushTile( ShowMapOnCamera.MAP_TILES[x,y] );
				}
			} else {
				ShowMapOnCamera.MAP_TILES[x,y] = this;
			}
		}
	}


	// Arrange the collider for this tile
	void SetCollider() {
        
        // Collider info from collisionData
        bc.enabled = true;
        char c = ShowMapOnCamera.S.collisionS[tileNum];
        switch (c) {
            case 'S': // Solid
                gameObject.tag = "Static";
                bc.center = Vector3.zero;
                bc.size = Vector3.one;
                break;
            //case 'P': //Pushable
            //    gameObject.tag = "Pushable";
            //    break;
            case 'L': //Locked
                gameObject.tag = "Locked";
                break;
            case 'D': //Door
                gameObject.tag = "Door";
                GetComponent<Collider>().isTrigger = true;
                //which door is it?
                if (this.tileNum == 48) {
                    bc.center = new Vector3(0.5f, 0, 0);
                    bc.size = new Vector3(0.5f, 1, 1);
                } else if (this.tileNum == 51) {
                    bc.center = new Vector3(-0.5f, 0, 0);
                    bc.size = new Vector3(-0.5f, 1, 1);
                } else if (this.tileNum == 26 || this.tileNum == 27) {
                    if (this.tileNum == 27) {
                        bc.center = new Vector3(-0.5f, -0.33f, 0);
                        bc.size = new Vector3(1.75f, -0.5f, 1);
                    } else {
                        bc.enabled = false;
                        GetComponent<Collider>().isTrigger = false;
                    }
                } else if (this.tileNum == 92 || this.tileNum == 93) {
                    if (this.tileNum == 93) {
                        bc.center = new Vector3(-0.5f, 0.33f, 0);
                        bc.size = new Vector3(1.75f, -0.5f, 1);
                    } else {
                        bc.enabled = false;
                        GetComponent<Collider>().isTrigger = false;
                    }

                }
                break;
            case 'W': // Water
            bc.center = Vector3.zero;
            bc.size = Vector3.one;
            gameObject.layer = LayerMask.NameToLayer("Water");
            break;
            default:
            bc.enabled = false;
            break;
        }
	}	
}