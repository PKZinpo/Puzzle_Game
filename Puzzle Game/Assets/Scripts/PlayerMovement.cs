using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour {

    public float moveUpSpeed;
    public float movementSpeed;
    public Tilemap ground;

    public static Vector3 currentPosition;

    private Vector3Int togridPos;
    private Vector3Int selectedgridPos;
    private bool bottomMove = false;
    private bool topMove = false;
    private bool onSecondFloor = false;

    private static Tilemap currentGround;
    private static Vector3 destination;
    private static Vector3 playerOffset;
    private static bool movetoDest = false;
    private static bool movingUp = false;

    void Awake() {
        playerOffset = new Vector3(0.0f, -0.01f, 0.0f);
        currentGround = ground;
        currentPosition = transform.parent.transform.position;
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
                }
                else {
                    transform.position = currentPosition;
                }
            }
        }
        //if (!TileMoving.isMoving) {
        //    if (!currentGround.HasTile(currentGround.WorldToCell(currentPosition))) {
        //        GMPlayer.ResetLevel();
        //    }
        //    if (temp != null) {
        //        transform.SetParent(transform.parent.parent);
        //        Destroy(temp);
        //    }
        //}
    }

    #region ArrowMovement
    public void moveplayerTL() {
        if (!TileMoving.isMoving) {
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
                    movetoDest = true;
                    topMove = true;
                }
            }
        }
    }
    public void moveplayerTR() {
        if (!TileMoving.isMoving) {
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
                    movetoDest = true;
                    topMove = true;
                }
            }
        }
    }
    public void moveplayerBR() {
        if (!TileMoving.isMoving) {
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
                    movetoDest = true;
                    bottomMove = true;
                }
            }
        }
    }
    public void moveplayerBL() {
        if (!TileMoving.isMoving) {
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
}
