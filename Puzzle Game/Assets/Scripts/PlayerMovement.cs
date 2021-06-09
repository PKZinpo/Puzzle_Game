using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 1f;
    public Tilemap ground;
    public Tilemap wall;

    public static Vector3 currentPosition;

    private GameObject temp = null;

    private static SpriteRenderer playerRenderer;
    private static Tilemap currentGround;
    private static Tilemap currentWall;
    private Vector3Int togridPos;
    private Vector3Int selectedgridPos;
    private static Vector3 destination;
    private static bool movetoDest = false;

    void Awake() {
        currentGround = ground;
        currentWall = wall;
        currentPosition = transform.position;
        playerRenderer = GetComponent<SpriteRenderer>();
        if (currentPosition != currentGround.CellToWorld(currentGround.WorldToCell(currentPosition))) {
            currentPosition = currentGround.CellToWorld(currentGround.WorldToCell(currentPosition));
            transform.position = currentPosition;
        }
    }

    
    void Update() {
        if (movetoDest) {
            var destinationGridPos = currentGround.WorldToCell(destination);
            var destinationWallPos = new Vector3Int(destinationGridPos.x + 1, destinationGridPos.y + 1, 0);
            if (currentWall.HasTile(destinationWallPos) && currentGround != currentWall) {
                Debug.Log("There is a Wall");
                movetoDest = false;
            }
            else {
                currentPosition = Vector3.MoveTowards(currentPosition, destination, movementSpeed * Time.deltaTime);
                if (currentPosition == destination) {
                    movetoDest = false;
                }
            }
            transform.position = currentPosition;
        }
        if (!TileMoving.isMoving) {
            if (!currentGround.HasTile(currentGround.WorldToCell(currentPosition))) {
                GMPlayer.ResetLevel();
            }
            if (temp != null) {
                transform.SetParent(transform.parent.parent);
                Destroy(temp);
            }
        }
    }

    #region ArrowMovement

    public void moveplayerTL() {
        if (!TileMoving.isMoving) {
            selectedgridPos = currentGround.WorldToCell(currentPosition);
            togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y + 1, 0);
            if (currentGround.HasTile(togridPos)) {
                destination = currentGround.CellToWorld(togridPos);
                destination = new Vector3(destination.x, destination.y, 0);
                movetoDest = true;
            }
        }
        
    }
    public void moveplayerTR() {
        if (!TileMoving.isMoving) {
            selectedgridPos = currentGround.WorldToCell(currentPosition);
            togridPos = new Vector3Int(selectedgridPos.x + 1, selectedgridPos.y, 0);
            if (currentGround.HasTile(togridPos)) {
                destination = currentGround.CellToWorld(togridPos);
                destination = new Vector3(destination.x, destination.y, 0);
                movetoDest = true;
            }
        }
    }
    public void moveplayerBR() {
        if (!TileMoving.isMoving) {
            selectedgridPos = currentGround.WorldToCell(currentPosition);
            togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y - 1, 0);
            if (currentGround.HasTile(togridPos)) {
                destination = currentGround.CellToWorld(togridPos);
                destination = new Vector3(destination.x, destination.y, 0);
                movetoDest = true;
            }
        }
    }
    public void moveplayerBL() {
        if (!TileMoving.isMoving) {
            selectedgridPos = currentGround.WorldToCell(currentPosition);
            togridPos = new Vector3Int(selectedgridPos.x - 1, selectedgridPos.y, 0);
            if (currentGround.HasTile(togridPos)) {
                destination = currentGround.CellToWorld(togridPos);
                destination = new Vector3(destination.x, destination.y, 0);
                movetoDest = true;
            }
        }
    }
    #endregion

    public static void MovePlayerUp(Vector3 playerPos) {
        currentPosition = playerPos;
        currentGround = currentWall;
        playerRenderer.sortingLayerName = "2nd Floor";
    }
}
