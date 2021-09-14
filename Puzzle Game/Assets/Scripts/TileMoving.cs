using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileMoving : MonoBehaviour {

    #region Variables

    public Tilemap currentGround;
    public Tile brokenPlatform;
    public Tile groundFullTile;
    public Tile groundHalfTile;
    public Tile groundIceTile;
    public Tile brokenIcePlatform;
    public GameObject brokenTile;
    public GameObject fullTile;
    public GameObject halfTile;
    public GameObject brokenIceTile;
    public GameObject iceTile;
    public GameObject tempWall;
    public GameObject playerObj;

    public static bool isMoving = false;
    public static Vector3 wallOffset = new Vector3(0.0f, -0.02f, 0.0f);

    private static GameObject brokengroundTile;
    private static GameObject fullgroundTile;
    private static GameObject halfgroundTile;
    private static GameObject brokengroundIceTile;
    private static GameObject halfgroundIceTile;
    private static GameObject tile = null;
    private static GameObject tileIce = null;
    private static GameObject wallTileTemp = null;
    private static GameObject wallObject = null;
    private static GameObject icewallObject = null;
    private static GameObject tempWallObject = null;
    private static GameObject player;
    private static Tilemap ground;
    private static Tile brokenPlat;
    private static Tile groundFullPlat;
    private static Tile groundHalfPlat;
    private static Tile groundHalfIcePlat;
    private static Tile brokenIcePlat;

    #endregion

    void Awake() {
        fullgroundTile = fullTile;
        halfgroundTile = halfTile;
        brokengroundTile = brokenTile;
        brokengroundIceTile = brokenIceTile;
        halfgroundIceTile = iceTile;
        ground = currentGround;
        brokenPlat = brokenPlatform;
        groundFullPlat = groundFullTile;
        groundHalfPlat = groundHalfTile;
        brokenIcePlat = brokenIcePlatform;
        groundHalfIcePlat = groundIceTile;
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

                        case "Broken Ice Tile":
                            tile = Instantiate(brokengroundIceTile);
                            CheckAdjacentIceTiles(tilePos[i], tilePos, isWall);
                            break;

                        case "Ice Tile":
                            tile = Instantiate(halfgroundIceTile);
                            CheckAdjacentIceTiles(tilePos[i], tilePos, isWall);
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

                        case "Tilemap Broken Ice":
                            tile = Instantiate(brokengroundIceTile);
                            CheckAdjacentIceTiles(tilePos[i], tilePos, isWall);
                            break;

                        case "Tilemap Ice":
                            tile = Instantiate(halfgroundIceTile);
                            CheckAdjacentIceTiles(tilePos[i], tilePos, isWall);
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
            else if (type.Contains("Broken Tile")) {
                wallTileTemp = Instantiate(brokengroundTile);
            }
            else if (type.Contains("Broken Ice")) {
                wallTileTemp = Instantiate(brokengroundIceTile);
            }
            else {
                wallTileTemp = Instantiate(halfgroundIceTile);
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
            else if (type.Contains("Broken Tile")) {
                ground.SetTile(ground.WorldToCell(position), brokenPlat);
            }
            else if (type.Contains("Broken Ice")) {
                ground.SetTile(ground.WorldToCell(position), brokenIcePlat);
            }
            else {
                ground.SetTile(ground.WorldToCell(position), groundHalfIcePlat);
            }
        }
    }
    private static void CheckAdjacentIceTiles(Vector3Int currentPos, Vector3Int[] tilePos, bool wall) {
        int limiter = 0, loopCap = 100;
        int prevlistNum;
        bool nomoreIce = false;
        List<Vector3Int> tempPos = new List<Vector3Int>();
        List<Vector3Int> icePositions = new List<Vector3Int>();
        
        for (int i = 0; i < tilePos.Length; i++) {
            tempPos.Add(tilePos[i]);
        }
        if (wall) {
            if (!tempPos.Contains(currentPos + Vector3Int.right)) {
                foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                    if (item.transform.position == ground.CellToWorld(currentPos + Vector3Int.right) + wallOffset) {
                        if (item.GetComponentInChildren<SpriteRenderer>().name.Contains("Ice")) {
                            icePositions.Add(currentPos + Vector3Int.right);
                            Debug.Log("POS X INITIAL WALL");
                            break;
                        }
                    }
                }
            }
            if (!tempPos.Contains(currentPos + Vector3Int.left)) {
                foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                    if (item.transform.position == ground.CellToWorld(currentPos + Vector3Int.left) + wallOffset) {
                        if (item.GetComponentInChildren<SpriteRenderer>().name.Contains("Ice")) {
                            icePositions.Add(currentPos + Vector3Int.left);
                            Debug.Log("NEG X INITIAL WALL");
                            break;
                        }
                    }
                }

            }
            if (!tempPos.Contains(currentPos + Vector3Int.up)) {
                foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                    if (item.transform.position == ground.CellToWorld(currentPos + Vector3Int.up) + wallOffset) {
                        if (item.GetComponentInChildren<SpriteRenderer>().name.Contains("Ice")) {
                            icePositions.Add(currentPos + Vector3Int.up);
                            Debug.Log("POS Y INITIAL WALL");
                            break;
                        }
                    }
                }

            }
            if (!tempPos.Contains(currentPos + Vector3Int.down)) {
                foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                    if (item.transform.position == ground.CellToWorld(currentPos + Vector3Int.down) + wallOffset) {
                        if (item.GetComponentInChildren<SpriteRenderer>().name.Contains("Ice")) {
                            icePositions.Add(currentPos + Vector3Int.down);
                            Debug.Log("NEG Y INITIAL WALL");
                            break;
                        }
                    }
                }
            }
        }
        else {
            if (!tempPos.Contains(currentPos + Vector3Int.right)) {
                if (ground.HasTile(currentPos + Vector3Int.right)) {
                    if (ground.GetTile(currentPos + Vector3Int.right).name.Contains("Ice")) {
                        icePositions.Add(currentPos + Vector3Int.right);
                        Debug.Log("POS X INITIAL FLOOR");
                    }
                }
            }
            if (!tempPos.Contains(currentPos + Vector3Int.left)) {
                if (ground.HasTile(currentPos + Vector3Int.left)) {
                    if (ground.GetTile(currentPos + Vector3Int.left).name.Contains("Ice")) {
                        icePositions.Add(currentPos + Vector3Int.left);
                        Debug.Log("NEG X INITIAL FLOOR");
                    }
                }
            }
            if (!tempPos.Contains(currentPos + Vector3Int.up)) {
                if (ground.HasTile(currentPos + Vector3Int.up)) {
                    if (ground.GetTile(currentPos + Vector3Int.up).name.Contains("Ice")) {
                        icePositions.Add(currentPos + Vector3Int.up);
                        Debug.Log("POS Y INITIAL FLOOR");
                    }
                }
            }
            if (!tempPos.Contains(currentPos + Vector3Int.down)) {
                if (ground.HasTile(currentPos + Vector3Int.down)) {
                    if (ground.GetTile(currentPos + Vector3Int.down).name.Contains("Ice")) {
                        icePositions.Add(currentPos + Vector3Int.down);
                        Debug.Log("NEG Y INITIAL FLOOR");
                    }
                }
            }
        }

        while (!nomoreIce && limiter < loopCap) {
            prevlistNum = icePositions.Count;
            for (int i = 0; i < icePositions.Count; i++) {
                if (wall) {
                    if (!tempPos.Contains(icePositions[i] + Vector3Int.right) && !icePositions.Contains(icePositions[i] + Vector3Int.right)) {
                        foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                            if (item.transform.position == ground.CellToWorld(icePositions[i] + Vector3Int.right) + wallOffset) {
                                if (item.GetComponentInChildren<SpriteRenderer>().name.Contains("Ice")) {
                                    icePositions.Add(icePositions[i] + Vector3Int.right);
                                    Debug.Log("POS X WALL");
                                    break;
                                }
                            }
                        }
                    }
                    if (!tempPos.Contains(icePositions[i] + Vector3Int.left) && !icePositions.Contains(icePositions[i] + Vector3Int.left)) {
                        foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                            if (item.transform.position == ground.CellToWorld(icePositions[i] + Vector3Int.left) + wallOffset) {
                                if (item.GetComponentInChildren<SpriteRenderer>().name.Contains("Ice")) {
                                    icePositions.Add(icePositions[i] + Vector3Int.left);
                                    Debug.Log("NEG X WALL");
                                    break;
                                }
                            }
                        }

                    }
                    if (!tempPos.Contains(icePositions[i] + Vector3Int.up) && !icePositions.Contains(icePositions[i] + Vector3Int.up)) {
                        foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                            if (item.transform.position == ground.CellToWorld(icePositions[i] + Vector3Int.up) + wallOffset) {
                                if (item.GetComponentInChildren<SpriteRenderer>().name.Contains("Ice")) {
                                    icePositions.Add(icePositions[i] + Vector3Int.up);
                                    Debug.Log("POS Y WALL");
                                    break;
                                }
                            }
                        }

                    }
                    if (!tempPos.Contains(icePositions[i] + Vector3Int.down) && !icePositions.Contains(icePositions[i] + Vector3Int.down)) {
                        foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                            if (item.transform.position == ground.CellToWorld(icePositions[i] + Vector3Int.down) + wallOffset) {
                                if (item.GetComponentInChildren<SpriteRenderer>().name.Contains("Ice")) {
                                    icePositions.Add(icePositions[i] + Vector3Int.down);
                                    Debug.Log("NEG Y WALL");
                                    break;
                                }
                            }
                        }
                    }
                }
                else {
                    if (!tempPos.Contains(icePositions[i] + Vector3Int.right) && !icePositions.Contains(icePositions[i] + Vector3Int.right)) {
                        if (ground.HasTile(icePositions[i] + Vector3Int.right)) {
                            if (ground.GetTile(icePositions[i] + Vector3Int.right).name.Contains("Ice")) {
                                icePositions.Add(icePositions[i] + Vector3Int.right);
                                Debug.Log("POS X EXTRA");
                            }
                        }
                    }
                    if (!tempPos.Contains(icePositions[i] + Vector3Int.left) && !icePositions.Contains(icePositions[i] + Vector3Int.left)) {
                        if (ground.HasTile(icePositions[i] + Vector3Int.left)) {
                            if (ground.GetTile(icePositions[i] + Vector3Int.left).name.Contains("Ice")) {
                                icePositions.Add(icePositions[i] + Vector3Int.left);
                                Debug.Log("NEG X EXTRA");
                            }
                        }
                    }
                    if (!tempPos.Contains(icePositions[i] + Vector3Int.up) && !icePositions.Contains(icePositions[i] + Vector3Int.up)) {
                        if (ground.HasTile(icePositions[i] + Vector3Int.up)) {
                            if (ground.GetTile(icePositions[i] + Vector3Int.up).name.Contains("Ice")) {
                                icePositions.Add(icePositions[i] + Vector3Int.up);
                                Debug.Log("POS Y EXTRA");
                            }
                        }
                    }
                    if (!tempPos.Contains(icePositions[i] + Vector3Int.down) && !icePositions.Contains(icePositions[i] + Vector3Int.down)) {
                        if (ground.HasTile(icePositions[i] + Vector3Int.down)) {
                            if (ground.GetTile(icePositions[i] + Vector3Int.down).name.Contains("Ice")) {
                                icePositions.Add(icePositions[i] + Vector3Int.down);
                                Debug.Log("NEG Y EXTRA");
                            }
                        }
                    }
                }
            }
            
            if (prevlistNum == icePositions.Count) {
                nomoreIce = true;
            }

            limiter++;
        }

        for (int i = 0; i < icePositions.Count; i++) {
            string tileType;
            bool isWall = false;
            foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
                if (item.transform.position == ground.CellToWorld(icePositions[i]) + wallOffset) {
                    isWall = true;
                    icewallObject = item;
                    Debug.Log(item);
                    break;
                }
            }
            if (isWall) {
                tileType = icewallObject.GetComponentInChildren<SpriteRenderer>().sprite.name;
                switch (tileType) {
                    case "Broken Ice Tile":
                        tileIce = Instantiate(brokengroundIceTile);
                        break;

                    case "Ice Tile":
                        tileIce = Instantiate(halfgroundIceTile);
                        break;
                }
                Destroy(icewallObject.transform.GetChild(0).gameObject);
                tileIce.transform.SetParent(icewallObject.transform);
                tileIce.GetComponent<TileProperties>().ChangeDisappearing();
                tileIce.transform.localPosition = new Vector3(0.0f, 0.16f, 0.0f);
            }
            if (ground.HasTile(icePositions[i])) {
                tileType = ground.GetTile(icePositions[i]).name;
                ground.SetTile(icePositions[i], null);
                switch (tileType) {
                    case "Tilemap Broken Ice":
                        tileIce = Instantiate(brokengroundIceTile);
                        break;

                    case "Tilemap Ice":
                        tileIce = Instantiate(halfgroundIceTile);
                        break;
                }
                tileIce.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                tileIce.transform.position = ground.CellToWorld(icePositions[i]);
                if (ground.WorldToCell(player.transform.position) == icePositions[i]) {
                    PlayerMovement.MovePlayerUp();
                }
            }
        }
    }
}
