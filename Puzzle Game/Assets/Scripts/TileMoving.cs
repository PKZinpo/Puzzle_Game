using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMoving : MonoBehaviour {

    public Tilemap currentGround;
    public Tilemap currentWall;
    public Tile brokenPlatform;
    public Tile groundFullTile;
    public Tile groundHalfTile;
    public Tile groundTile;
    public Tile wallTile;
    public GameObject brokenTile;
    public GameObject fullTile;
    public GameObject halfTile;
    public GameObject tilePrefab;
    public GameObject player;

    public static bool isMoving = false;

    private static GameObject brokengroundTile;
    private static GameObject fullgroundTile;
    private static GameObject halfgroundTile;
    private static GameObject tile = null;
    private static Tilemap ground;
    private static Tilemap wall;
    private static Tile brokenPlat;
    private static Tile groundFullPlat;
    private static Tile groundHalfPlat;


    void Awake() {
        fullgroundTile = fullTile;
        halfgroundTile = halfTile;
        brokengroundTile = brokenTile;
        ground = currentGround;
        wall = currentWall;
        brokenPlat = brokenPlatform;
        groundFullPlat = groundFullTile;
        groundHalfPlat = groundHalfTile;
    }

    
    void Update() {
        if (isMoving) {
            if (GameObject.FindGameObjectsWithTag("Tile").Length == 0) {
                isMoving = false;
            }
        }

    }

    public static void MoveTiles(Vector3Int[] tilePos) {
        for (int i = 0; i < tilePos.Length; i++) {
            string tileType;
            var wallPos = new Vector3Int(tilePos[i].x + 1, tilePos[i].y + 1, 0);
            if (wall.HasTile(wallPos)) {
                tileType = wall.GetTile(wallPos).name;
                if (tileType == "Ground Exit Tile") {
                    Debug.Log("Cannot move Tile");
                }
                else {
                    wall.SetTile(wallPos, null);
                    switch (tileType) {
                        case "Ground Full Tile":
                            tile = Instantiate(fullgroundTile);
                            break;

                        case "Ground Half Tile":
                            tile = Instantiate(halfgroundTile);
                            break;

                        case "Broken Tile":
                            tile = Instantiate(brokengroundTile);
                            break;
                    }
                    tile.GetComponent<TileProperties>().ChangeDisappearing();
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                    tile.transform.position = wall.CellToWorld(wallPos);
                }
            }
            else if (ground.HasTile(tilePos[i])) {
                tileType = ground.GetTile(tilePos[i]).name;
                if (tileType == "Ground Exit Tile") {
                    Debug.Log("Cannot move Tile");
                }
                else {
                    ground.SetTile(tilePos[i], null);
                    switch (tileType) {
                        case "Ground Full Tile":
                            tile = Instantiate(fullgroundTile);
                            break;

                        case "Ground Half Tile":
                            tile = Instantiate(halfgroundTile);
                            break;

                        case "Broken Tile":
                            tile = Instantiate(brokengroundTile);
                            tile.GetComponent<TileProperties>().ChangeMoveUp();
                            break;
                    }
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                    tile.transform.position = ground.CellToWorld(tilePos[i]);
                }
            }
            else {
                ground.SetTile(tilePos[i], null);
                tile = Instantiate(brokengroundTile);
                tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                tile.transform.position = ground.CellToWorld(tilePos[i]);
            }
        }
        isMoving = true;
    }

    public static void PlaceTiles(Vector3 position, string layer, string type) {
        if (layer == "Wall") {
            var gridPos = wall.WorldToCell(position);
            var wallPos = new Vector3Int(gridPos.x + 1, gridPos.y + 1, 0);
            if (type.Contains("Ground Full")) {
                wall.SetTile(wallPos, groundFullPlat);
            }
            else if (type.Contains("Ground Half")) {
                wall.SetTile(wallPos, groundHalfPlat);
            }
            else {
                wall.SetTile(wallPos, brokenPlat);
            }
        }
        else {
            if (type.Contains("Ground Full")) {
                ground.SetTile(ground.WorldToCell(position), groundFullPlat);
            }
            else if (type.Contains("Ground Half")) {
                ground.SetTile(ground.WorldToCell(position), groundHalfPlat);
            }
            else {
                ground.SetTile(ground.WorldToCell(position), brokenPlat);
            }
        }
    }
}
