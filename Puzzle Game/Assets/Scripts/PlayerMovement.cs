using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 1f;
    public Tilemap ground;
    public Tilemap wall;

    private static SpriteRenderer playerRenderer;
    private static Tilemap currentGround;
    private static Tilemap currentWall;
    private static Vector3 currentPosition;
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
        }
        transform.position = currentPosition;
    }

    #region ArrowMovement

    public void moveplayerTL() {
        selectedgridPos = currentGround.WorldToCell(currentPosition);
        togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y + 1, 0);
        if (currentGround.HasTile(togridPos)) {
            destination = currentGround.CellToWorld(togridPos);
            destination = new Vector3(destination.x, destination.y, 0);
            movetoDest = true;
        }
    }
    public void moveplayerTR() {
        selectedgridPos = currentGround.WorldToCell(currentPosition);
        togridPos = new Vector3Int(selectedgridPos.x + 1, selectedgridPos.y, 0);
        if (currentGround.HasTile(togridPos)) {
            destination = currentGround.CellToWorld(togridPos);
            destination = new Vector3(destination.x, destination.y, 0);
            movetoDest = true;
        }
    }
    public void moveplayerBR() {
        selectedgridPos = currentGround.WorldToCell(currentPosition);
        togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y - 1, 0);
        if (currentGround.HasTile(togridPos)) {
            destination = currentGround.CellToWorld(togridPos);
            destination = new Vector3(destination.x, destination.y, 0);
            movetoDest = true;
        }
    }
    public void moveplayerBL() {
        selectedgridPos = currentGround.WorldToCell(currentPosition);
        togridPos = new Vector3Int(selectedgridPos.x - 1, selectedgridPos.y, 0);
        if (currentGround.HasTile(togridPos)) {
            destination = currentGround.CellToWorld(togridPos);
            destination = new Vector3(destination.x, destination.y, 0);
            movetoDest = true;
        }
    }
    #endregion

    public static void MovePlayerUp(Vector3Int ground, Vector3Int wall) {
        if (currentGround.CellToWorld(ground) == currentPosition) {
            currentGround = currentWall;
            currentPosition = currentGround.CellToWorld(wall);
            playerRenderer.sortingLayerName = "2nd Floor";
        }
    }
}
