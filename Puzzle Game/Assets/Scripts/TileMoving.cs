using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;

public class TileMoving : MonoBehaviour {

    public Tilemap currentGround;
    public Tilemap currentWall;
    public Tile groundTile;
    public Tile wallTile;
    public GameObject tilePrefab;
    public GameObject player;
    public float tileSpeed = 0.5f;

    public static bool isMoving = false;

    private static GameObject temp = null;
    private static GameObject tileTemp;
    private static GameObject tile = null;
    private static Tilemap ground;
    private static Tilemap wall;
    private static float tileDistance;

    private List<Vector3> destinations = new List<Vector3>();
    private float fadeSpeed = 5f;


    void Awake() {
        tileTemp = tilePrefab;
        ground = currentGround;
        wall = currentWall;

        tileDistance = currentWall.CellToWorld(new Vector3Int(1, 1, 0)).y - currentGround.CellToWorld(new Vector3Int(0, 0, 0)).y;
    }

    
    void Update() {
        if (isMoving) {
            if (GameObject.FindGameObjectsWithTag("Tile").Length != 0) {
                if (destinations.Count == 0) {
                    int i = 0;
                    foreach (var item in GameObject.FindGameObjectsWithTag("Tile")) {
                        if (item.GetComponentInParent<SortingGroup>().sortingLayerName == "Ground") {

                        }
                        var tempDest = new Vector3(item.transform.position.x, item.transform.position.y + tileDistance, 0);
                        destinations.Add(tempDest);
                        i++;
                    }
                }
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("Tile").Length; i++) {
                    var item = GameObject.FindGameObjectsWithTag("Tile")[i];
                    if (item.GetComponentInParent<SortingGroup>().sortingLayerName == "Ground") {
                        if (ground.WorldToCell(player.transform.position) == ground.WorldToCell(item.transform.parent.transform.position)) {
                            item.transform.position = Vector3.MoveTowards(item.transform.position, destinations[i], tileSpeed * Time.deltaTime);
                            player.transform.position = item.transform.position;
                        }
                        else {
                            item.transform.position = Vector3.MoveTowards(item.transform.position, destinations[i], tileSpeed * Time.deltaTime);
                        }
                        if (i == GameObject.FindGameObjectsWithTag("Tile").Length - 1) {
                            if (item.transform.position == destinations[i]) {
                                NewTiles();
                                foreach (var tileObject in GameObject.FindGameObjectsWithTag("TileTemp")) {
                                    Destroy(tileObject);
                                }
                                destinations.Clear();
                                isMoving = false;
                            }
                        }
                    }
                    else {
                        var color = item.GetComponent<SpriteRenderer>().color;
                        if (color.a > 0) {
                            color.a -= fadeSpeed * Time.deltaTime;
                            item.GetComponent<SpriteRenderer>().color = color;
                        }
                    }
                }
            }
        }
    }

    public static void MoveTiles(Vector3Int[] tilePos) {
        for (int i = 0; i < tilePos.Length; i++) {
            string tileType;
            var wallPos = new Vector3Int(tilePos[i].x + 1, tilePos[i].y + 1, 0);
            if (wall.HasTile(wallPos)) {
                tileType = wall.GetTile(wallPos).name;
                wall.SetTile(wallPos, null);
                foreach (var item in StatueData.tileSprites) {
                    if (tileType == item.name) {
                        temp = new GameObject();
                        temp.tag = "TileTemp";
                        temp.AddComponent<SortingGroup>();
                        temp.GetComponent<SortingGroup>().sortingLayerName = "1st Floor";
                        tile = Instantiate(tileTemp);
                        tile.transform.SetParent(temp.transform);
                        temp.transform.position = wall.CellToWorld(wallPos);
                        tile.transform.position = temp.transform.position;
                        tile.GetComponent<SpriteRenderer>().sprite = item;
                        break;
                    }
                }
            }
            else if (ground.HasTile(tilePos[i])) {
                tileType = ground.GetTile(tilePos[i]).name;
                ground.SetTile(tilePos[i], null);
                foreach (var item in StatueData.tileSprites) {
                    if (tileType == item.name) {
                        temp = new GameObject();
                        temp.tag = "TileTemp";
                        temp.AddComponent<SortingGroup>();
                        temp.GetComponent<SortingGroup>().sortingLayerName = "Ground";
                        tile = Instantiate(tileTemp);
                        tile.transform.SetParent(temp.transform);
                        temp.transform.position = ground.CellToWorld(tilePos[i]);
                        tile.transform.position = temp.transform.position;
                        tile.GetComponent<SpriteRenderer>().sprite = item;
                        break;
                    }
                }
            }
            else {
                tileType = "Isometric Tile";
                ground.SetTile(tilePos[i], null);
                foreach (var item in StatueData.tileSprites) {
                    if (tileType == item.name) {
                        temp = new GameObject();
                        temp.tag = "TileTemp";
                        temp.AddComponent<SortingGroup>();
                        temp.GetComponent<SortingGroup>().sortingLayerName = "Ground";
                        tile = Instantiate(tileTemp);
                        tile.transform.SetParent(temp.transform);
                        temp.transform.position = ground.CellToWorld(tilePos[i]);
                        tile.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y - tileDistance, 0);
                        tile.GetComponent<SpriteRenderer>().sprite = item;
                        break;
                    }
                }
            }
        }
        isMoving = true;
    }
    public void NewTiles() {
        foreach (var tiles in GameObject.FindGameObjectsWithTag("Tile")) {
            if (tiles.GetComponentInParent<SortingGroup>().sortingLayerName == "Ground") {
                if (tiles.transform.localPosition.y > 0.1) {
                    wall.SetTile(wall.WorldToCell(tiles.transform.position), wallTile);
                    if (player.transform.position == tiles.transform.position && player.transform.position != PlayerMovement.currentPosition) {
                        PlayerMovement.MovePlayerUp(player.transform.position);
                    }
                }
                else {
                    ground.SetTile(ground.WorldToCell(tiles.transform.position), groundTile);
                }
            }
        }
    }
}
