using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour {

    #region Variables

    public float moveUpSpeed;
    public float movementSpeed;
    public Tilemap ground;
    public GameObject brokenTileObject;

    public static Vector3 currentPosition;

    private Vector3Int togridPos;
    private Vector3Int selectedgridPos;
    private Vector3Int prevgridPos;
    private bool bottomMove = false;
    private bool topMove = false;
    private bool onSecondFloor = false;

    private static Tilemap currentGround;
    private static Vector3 destination;
    private static Vector3 playerOffset;
    private static bool movetoDest = false;
    private static bool movingUp = false;

    #endregion

    void Awake() {
        playerOffset = new Vector3(0.0f, -0.01f, 0.0f);
        currentGround = ground;
        currentPosition = transform.parent.transform.position;
        prevgridPos = currentGround.WorldToCell(currentPosition);
        if (currentPosition != currentGround.CellToWorld(currentGround.WorldToCell(currentPosition))) {
            currentPosition = currentGround.CellToWorld(currentGround.WorldToCell(currentPosition)) + playerOffset;
            transform.parent.transform.position = currentPosition;
            transform.position -= playerOffset;
        }
    }

    
    void Update() {
        if (movingUp) {
            var moveUpDis = playerOffset + new Vector3(0.0f, -0.16f, 0.0f);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, -moveUpDis, moveUpSpeed * Time.deltaTime);
            if (transform.localPosition == -moveUpDis) {
                playerOffset = moveUpDis;
                currentPosition = transform.position;
                onSecondFloor = true;
                movingUp = false;
                transform.GetComponentInParent<SortingGroup>().sortingLayerName = "1st Floor";
            }
        }
        if (movetoDest) {
            if (topMove) {
                currentPosition = Vector3.MoveTowards(currentPosition, destination, movementSpeed * Time.deltaTime);
                if (currentPosition == destination) {
                    transform.parent.transform.position = currentPosition + playerOffset;
                    transform.position = transform.parent.transform.position - playerOffset;
                    movetoDest = false;
                    topMove = false;
                    CheckBrokenTile();
                    CheckTileUnder();
                }
                else {
                    transform.position = currentPosition;
                }
            }
            else if (bottomMove) {
                if (transform.parent.transform.position != destination + playerOffset) {
                    transform.parent.transform.position = destination + playerOffset;
                    transform.position = currentPosition;
                }
                currentPosition = Vector3.MoveTowards(currentPosition, destination, movementSpeed * Time.deltaTime);
                if (currentPosition == destination) {
                    movetoDest = false;
                    bottomMove = false;
                    CheckBrokenTile();
                    CheckTileUnder();
                }
                else {
                    transform.position = currentPosition;
                }
            }
        }
    }

    #region ArrowMovement
    public void moveplayerTL() {
        if (!movetoDest) {
            if (onSecondFloor) {
                selectedgridPos = currentGround.WorldToCell(transform.parent.transform.position - playerOffset);
                togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y + 1, 0);
                foreach (var item in GameObject.FindGameObjectsWithTag("Tile")) {
                    if (item.transform.position == currentGround.CellToWorld(togridPos)) {
                        destination = currentGround.CellToWorld(togridPos);
                        destination = new Vector3(destination.x, destination.y, 0);
                        movetoDest = true;
                        topMove = true;
                    }
                }
            }
            else {
                selectedgridPos = currentGround.WorldToCell(transform.parent.transform.position - playerOffset);
                togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y + 1, 0);
                if (currentGround.HasTile(togridPos)) {
                    destination = currentGround.CellToWorld(togridPos);
                    destination = new Vector3(destination.x, destination.y, 0);
                    CheckIceTile(destination);
                    movetoDest = true;
                    topMove = true;
                }
            }
        }
    }
    public void moveplayerTR() {
        if (!movetoDest) {
            if (onSecondFloor) {
                selectedgridPos = currentGround.WorldToCell(transform.parent.transform.position - playerOffset);
                togridPos = new Vector3Int(selectedgridPos.x + 1, selectedgridPos.y, 0);
                foreach (var item in GameObject.FindGameObjectsWithTag("Tile")) {
                    if (item.transform.position == currentGround.CellToWorld(togridPos)) {
                        destination = currentGround.CellToWorld(togridPos);
                        destination = new Vector3(destination.x, destination.y, 0);
                        movetoDest = true;
                        topMove = true;
                    }
                }
            }
            else {
                selectedgridPos = currentGround.WorldToCell(transform.parent.transform.position - playerOffset);
                togridPos = new Vector3Int(selectedgridPos.x + 1, selectedgridPos.y, 0);
                if (currentGround.HasTile(togridPos)) {
                    destination = currentGround.CellToWorld(togridPos);
                    destination = new Vector3(destination.x, destination.y, 0);
                    CheckIceTile(destination);
                    movetoDest = true;
                    topMove = true;
                }
            }
        }
    }
    public void moveplayerBR() {
        if (!movetoDest) {
            if (onSecondFloor) {
                selectedgridPos = currentGround.WorldToCell(transform.parent.transform.position - playerOffset);
                togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y - 1, 0);
                foreach (var item in GameObject.FindGameObjectsWithTag("Tile")) {
                    if (item.transform.position == currentGround.CellToWorld(togridPos)) {
                        destination = currentGround.CellToWorld(togridPos);
                        destination = new Vector3(destination.x, destination.y, 0);
                        movetoDest = true;
                        topMove = true;
                    }
                }
            }
            else {
                selectedgridPos = currentGround.WorldToCell(transform.parent.transform.position - playerOffset);
                togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y - 1, 0);
                if (currentGround.HasTile(togridPos)) {
                    destination = currentGround.CellToWorld(togridPos);
                    destination = new Vector3(destination.x, destination.y, 0);
                    CheckIceTile(destination);
                    movetoDest = true;
                    bottomMove = true;
                }
            }
        }
    }
    public void moveplayerBL() {
        if (!movetoDest) {
            if (onSecondFloor) {
                selectedgridPos = currentGround.WorldToCell(transform.parent.transform.position - playerOffset);
                togridPos = new Vector3Int(selectedgridPos.x - 1, selectedgridPos.y, 0);
                foreach (var item in GameObject.FindGameObjectsWithTag("Tile")) {
                    if (item.transform.position == currentGround.CellToWorld(togridPos)) {
                        destination = currentGround.CellToWorld(togridPos);
                        destination = new Vector3(destination.x, destination.y, 0);
                        movetoDest = true;
                        topMove = true;
                    }
                }
            }
            else {
                selectedgridPos = currentGround.WorldToCell(transform.parent.transform.position - playerOffset);
                togridPos = new Vector3Int(selectedgridPos.x - 1, selectedgridPos.y, 0);
                if (currentGround.HasTile(togridPos)) {
                    destination = currentGround.CellToWorld(togridPos);
                    destination = new Vector3(destination.x, destination.y, 0);
                    CheckIceTile(destination);
                    movetoDest = true;
                    bottomMove = true;
                }
            }
        }
    }
    #endregion

    public static void MovePlayerUp() {
        if (!movingUp) {
            movingUp = true;
        }
    }
    public void CheckBrokenTile() {
        if (onSecondFloor) {
            foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
                if (tile.transform.position == currentGround.CellToWorld(prevgridPos)) {
                    if (tile.name.Contains("Broken")) {
                        Destroy(tile);
                        GameObject temp = Instantiate(brokenTileObject);
                        temp.transform.position = currentGround.CellToWorld(prevgridPos);
                        temp.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                        temp.GetComponent<TileProperties>().ChangeMoveUp();
                        temp.GetComponent<TileProperties>().ChangeDisappearing();
                        break;
                    }
                }
            }
        }
        else {
            if (currentGround.GetTile(prevgridPos).name.Contains("Broken")) {
                currentGround.SetTile(prevgridPos, null);
                GameObject temp = Instantiate(brokenTileObject);
                temp.transform.position = currentGround.CellToWorld(prevgridPos);
                temp.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                temp.GetComponent<TileProperties>().ChangeMoveUp();
                temp.GetComponent<TileProperties>().ChangeDisappearing();
            }
        }
        prevgridPos = currentGround.WorldToCell(currentPosition);
    }
    public void CheckIceTile(Vector3 dest) {
        Vector3Int currentPos = currentGround.WorldToCell(transform.position);
        Vector3Int currentDestination = currentGround.WorldToCell(dest);
        Vector3Int direction = currentDestination - currentPos;
        while (currentGround.GetTile(currentDestination).name.Contains("Ice")) {
            currentDestination = currentDestination + direction;
            if (currentGround.GetTile(currentDestination) == null) {
                break;
            }
        }
        destination = currentGround.CellToWorld(currentDestination);
    }
    public void CheckTileUnder() {
        int tileCheck = 0;
        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
            if (tile.transform.position == transform.position) {
                tileCheck++;
            }
        }
        if (currentGround.GetTile(currentGround.WorldToCell(transform.position)) == null && tileCheck == 0) {
            GMPlayer.ResetLevel();
        }
    }
}
