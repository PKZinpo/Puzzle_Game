using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMoving : MonoBehaviour {

    #region Variables

    public Tilemap currentGround;
    public Tile brokenPlatform;
    public Tile groundFullTile;
    public Tile groundHalfTile;
    public GameObject brokenTile;
    public GameObject fullTile;
    public GameObject halfTile;
    public GameObject tempWall;
    public GameObject playerObj;

    public static bool isMoving = false;
    public static Vector3 wallOffset = new Vector3(0.0f, -0.02f, 0.0f);

    private static GameObject brokengroundTile;
    private static GameObject fullgroundTile;
    private static GameObject halfgroundTile;
    private static GameObject tile = null;
    private static GameObject wallTileTemp = null;
    private static GameObject wallObject = null;
    private static GameObject tempWallObject = null;
    private static GameObject player;
    private static Tilemap ground;
    private static Tile brokenPlat;
    private static Tile groundFullPlat;
    private static Tile groundHalfPlat;

    #endregion

    void Awake() {
        fullgroundTile = fullTile;
        halfgroundTile = halfTile;
        brokengroundTile = brokenTile;
        ground = currentGround;
        brokenPlat = brokenPlatform;
        groundFullPlat = groundFullTile;
        groundHalfPlat = groundHalfTile;
        tempWallObject = tempWall;
        player = playerObj;

        if (GameObject.FindGameObjectsWithTag("Wall").Length != 0) {
            foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                if (item.transform.position != currentGround.CellToWorld(currentGround.WorldToCell(item.transform.position))) {
                    Vector3 newPos = new Vector3(item.transform.position.x, item.transform.position.y, 0);
                    item.transform.position = currentGround.CellToWorld(currentGround.WorldToCell(newPos)) + wallOffset;
                    item.transform.GetChild(0).transform.position += new Vector3 (wallOffset.x, Mathf.Abs(wallOffset.y), wallOffset.z);
                }
            }
        }
    }
    void Update() {
        if (isMoving) {
            if (GameObject.FindGameObjectsWithTag("Tile").Length != 0) {
                var stationaryNum = 0;
                foreach (var item in GameObject.FindGameObjectsWithTag("Tile")) {
                    Animator tileAnim = item.GetComponent<Animator>();
                    if (tileAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Stationary")) {
                        stationaryNum++;
                    }
                }
                if (stationaryNum == GameObject.FindGameObjectsWithTag("Tile").Length) {
                    isMoving = false;
                }
            }
            else if (GameObject.FindGameObjectsWithTag("Tile").Length == 0) {
                isMoving = false;
            }
        }
    }
    public static void MoveTiles(Vector3Int[] tilePos) {
        for (int i = 0; i < tilePos.Length; i++) {
            string tileType;
            bool isWall = false;
            foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                if (item.transform.position == ground.CellToWorld(tilePos[i]) + wallOffset) {
                    isWall = true;
                    wallObject = item;
                }
            }
            if (isWall) {
                tileType = wallObject.GetComponentInChildren<SpriteRenderer>().sprite.name;
                if (tileType.Contains("Exit Tile")) {
                    Debug.Log("Cannot move Tile");
                }
                else {
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
                    Destroy(wallObject.transform.GetChild(0).gameObject);
                    tile.transform.SetParent(wallObject.transform);
                    tile.GetComponent<TileProperties>().ChangeDisappearing();
                    tile.transform.localPosition = new Vector3 (0.0f, 0.16f, 0.0f);
                }
            }
            else if (ground.HasTile(tilePos[i])) {
                tileType = ground.GetTile(tilePos[i]).name;
                if (tileType.Contains("Exit Tile")) {
                    Debug.Log("Cannot move Tile");
                }
                else {
                    ground.SetTile(tilePos[i], null);
                    switch (tileType) {
                        case "Tilemap Ground Full":
                            tile = Instantiate(fullgroundTile);
                            break;

                        case "Tilemap Ground Half":
                            tile = Instantiate(halfgroundTile);
                            break;

                        case "Tilemap Broken":
                            tile = Instantiate(brokengroundTile);
                            tile.GetComponent<TileProperties>().ChangeMoveUp();
                            break;
                    }
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                    tile.transform.position = ground.CellToWorld(tilePos[i]);
                    if (ground.WorldToCell(player.transform.position) == tilePos[i]) {
                        PlayerMovement.MovePlayerUp();
                    }
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
            var gridPos = ground.WorldToCell(position);
            var wallPos = new Vector3Int(gridPos.x + 1, gridPos.y + 1, 0);
            GameObject temp = Instantiate(tempWallObject);
            temp.transform.position = position + wallOffset;
            if (type.Contains("Ground Full")) {
                wallTileTemp = Instantiate(fullgroundTile);
            }
            else if (type.Contains("Ground Half")) {
                wallTileTemp = Instantiate(halfgroundTile);
            }
            else {
                wallTileTemp = Instantiate(brokengroundTile);
            }
            wallTileTemp.transform.SetParent(temp.transform);
            wallTileTemp.transform.position = ground.CellToWorld(wallPos);
            wallTileTemp.GetComponent<Animator>().SetTrigger("IsTile");
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
