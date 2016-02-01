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
                bc.isTrigger = false;
                break;
            case 'L': //Locked
                gameObject.tag = "Locked";
                bc.size = Vector3.one;
                bc.center = Vector3.zero;
                bc.isTrigger = false;
                if (this.tileNum == 101)
                    gameObject.name = "locked_R";
                else if (this.tileNum == 106)
                    gameObject.name = "locked_L";
                else if (this.tileNum == 80)
                    gameObject.name = "locked_TL";
                else if (this.tileNum == 81)
                    gameObject.name = "locked_TR";
                break;
            case 'D': //Door
                gameObject.tag = "Door";
                bc.isTrigger = true;
                //which door is it?
                if (this.tileNum == 48) {           //right
                    bc.center = new Vector3(0.5f, 0, 0);
                    bc.size = new Vector3(0.5f, 1, 1);
                }
                else if (this.tileNum == 51) {      //left
                    bc.center = new Vector3(-0.5f, 0, 0);
                    bc.size = new Vector3(-0.5f, 1, 1);
                }
                else if (this.tileNum == 26 || this.tileNum == 27) {        //bottom left / bottom right
                    if (this.tileNum == 27) {
                        bc.center = new Vector3(-0.5f, -0.33f, 0);
                        bc.size = new Vector3(1.75f, -0.5f, 1);
                    } else {
                        bc.enabled = false;
                        bc.isTrigger = false;
                    }
                }
                else if (this.tileNum == 92 || this.tileNum == 93) {        //top left / top right
                    if (this.tileNum == 93) {
                        bc.center = new Vector3(-0.5f, 0.33f, 0);
                        bc.size = new Vector3(1.75f, -0.5f, 1);
                    } else {
                        bc.enabled = false;
                        bc.isTrigger = false;
                    }

                }
                break;
            case 'W': // Water
                bc.center = new Vector3(0,0.25f);
                bc.size = new Vector3(1,0.5f,1);
                gameObject.tag = "Water";
                gameObject.layer = LayerMask.NameToLayer("Water");
                break;
            case 'V': // Lava
                bc.center = new Vector3(0, 0.25f);
                bc.size = new Vector3(1, 0.5f, 1);
                gameObject.tag = "Lava";
                gameObject.layer = LayerMask.NameToLayer("Lava");
                break;
            case 'O': // Solid block that's not a wall
                bc.center = new Vector3(0, 0.25f);
                bc.size = new Vector3(1, 0.5f, 1);
                gameObject.tag = "Static";
                break;
            case 'T': // sTairs
                gameObject.tag = "Stairs";
                bc.isTrigger = true;
                bc.center = Vector3.zero;
                bc.size = Vector3.one;
                break;
            default:
                bc.enabled = false;
                break;
        }
	}	
}