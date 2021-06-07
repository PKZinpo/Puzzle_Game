using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMovement {
    public string direction;
}


public class TileMoving : MonoBehaviour {

    public Tilemap currentGround;
    public Tilemap currentWall;
    public GameObject tilePrefab;
    public float tileSpeed = 0.5f;

    private bool isMoving = false;

    private static GameObject tileTemp;
    private static GameObject tile = null;
    private static Tilemap ground;
    private static Tilemap wall;
    private static List<Sprite> tileLocations = new List<Sprite>();



    void Awake() {
        tileTemp = tilePrefab;
        ground = currentGround;
        wall = currentWall;
    }

    
    void Update() {
        
    }

    public static void MoveTiles(Vector3Int[] tilePos) {
        for (int i = 0; i < tilePos.Length; i++) {
            string tileType;
            var wallPos = new Vector3Int(tilePos[i].x + 1, tilePos[i].y + 1, 0);
            if (wall.HasTile(wallPos)) {
                tileType = wall.GetTile(tilePos[i]).name;
                wall.SetTile(wallPos, null);
                foreach (var item in StatueData.tileSprites) {
                    if (tileType == item.name) {
                        tile = Instantiate(tileTemp);
                        tile.transform.position = wall.CellToWorld(wallPos);
                        tile.GetComponent<SpriteRenderer>().sortingLayerName = "1st Floor";
                        tile.GetComponent<SpriteRenderer>().sprite = item;
                    }
                }
            }
            else {
                tileType = ground.GetTile(tilePos[i]).name;
                ground.SetTile(tilePos[i], null);
                foreach (var item in StatueData.tileSprites) {
                    if (tileType == item.name) {
                        tile = Instantiate(tileTemp);
                        tile.transform.position = ground.CellToWorld(tilePos[i]);
                        tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                        tile.GetComponent<SpriteRenderer>().sprite = item;
                    }
                }
            }
        }
    }
}
