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
    public static bool movetoDest = false;

    private Vector3Int togridPos;
    private Vector3Int selectedgridPos;
    private Vector3Int prevgridPos;
    private bool bottomMove = false;
    private bool topMove = false;
    private bool onSecondFloor = false;
    private bool onIce = false;
    private AudioSource playerMove;
    private AudioSource iceSlide;

    private static Tilemap currentGround;
    private static Vector3 destination;
    private static Vector3 playerOffset;
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
    private void Start() {
        if (playerMove == null) {
            for (int i = 0; i < FindObjectOfType<AudioManager>().GetComponents<AudioSource>().Length; i++) {
                if (FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i].clip.name == "Statue Move (TEST)") {
                    playerMove = FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i];
                }
                if (FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i].clip.name == "IceSlide(TEST)") {
                    iceSlide = FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i];
                }
            }
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
                prevgridPos = currentGround.WorldToCell(currentPosition);
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

            if (onIce) {
                
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
                        CheckIceTile(destination);
                        movetoDest = true;
                        topMove = true;
                        FindObjectOfType<AudioManager>().Play("StatueMove");
                        playerMove.volume = 0.5f;
                        playerMove.pitch = 1.1f;
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
                    FindObjectOfType<AudioManager>().Play("StatueMove");
                    playerMove.volume = 0.5f;
                    playerMove.pitch = 1.1f;
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
                        CheckIceTile(destination);
                        movetoDest = true;
                        topMove = true;
                        FindObjectOfType<AudioManager>().Play("StatueMove");
                        playerMove.volume = 0.5f;
                        playerMove.pitch = 1.1f;
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
                    FindObjectOfType<AudioManager>().Play("StatueMove");
                    playerMove.volume = 0.5f;
                    playerMove.pitch = 1.1f;
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
                        CheckIceTile(destination);
                        movetoDest = true;
                        topMove = true;
                        FindObjectOfType<AudioManager>().Play("StatueMove");
                        playerMove.volume = 0.5f;
                        playerMove.pitch = 1.1f;
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
                    FindObjectOfType<AudioManager>().Play("StatueMove");
                    playerMove.volume = 0.5f;
                    playerMove.pitch = 1.1f;
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
                        CheckIceTile(destination);
                        movetoDest = true;
                        topMove = true;
                        FindObjectOfType<AudioManager>().Play("StatueMove");
                        playerMove.volume = 0.5f;
                        playerMove.pitch = 1.1f;
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
                    FindObjectOfType<AudioManager>().Play("StatueMove");
                    playerMove.volume = 0.5f;
                    playerMove.pitch = 1.1f;
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
                    if (tile.name.Contains("Broken") && !tile.name.Contains("Ice")) {
                        GameObject temp = Instantiate(brokenTileObject);
                        temp.transform.parent = tile.transform.parent;
                        Destroy(tile);
                        temp.transform.localPosition = new Vector3(0.0f, 0.16f, 0.0f);
                        temp.GetComponent<TileProperties>().ChangeMoveUp();
                        temp.GetComponent<TileProperties>().ChangeDisappearing();
                        FindObjectOfType<AudioManager>().Play("BrokenTileFall");
                        break;
                    }
                }
            }
        }
        else {
            if (currentGround.GetTile(prevgridPos).name.Contains("Broken") && !currentGround.GetTile(prevgridPos).name.Contains("Ice")) {
                currentGround.SetTile(prevgridPos, null);
                GameObject temp = Instantiate(brokenTileObject);
                temp.transform.position = currentGround.CellToWorld(prevgridPos);
                temp.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                temp.GetComponent<TileProperties>().ChangeMoveUp();
                temp.GetComponent<TileProperties>().ChangeDisappearing();
                FindObjectOfType<AudioManager>().Play("BrokenTileFall");
            }
        }
        prevgridPos = currentGround.WorldToCell(currentPosition);
    }
    public void CheckIceTile(Vector3 dest) {
        Vector3Int currentPos = currentGround.WorldToCell(transform.position);
        Vector3Int currentDestination = currentGround.WorldToCell(dest);
        Vector3Int direction = currentDestination - currentPos;
        if (onSecondFloor) {
            bool noIce = false;
            while (!noIce) {
                foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
                    if (tile.transform.position == currentGround.CellToWorld(currentDestination)) {
                        if (tile.name.Contains("Ice")) {
                            currentDestination = currentDestination + direction;
                            onIce = true;
                            FindObjectOfType<AudioManager>().Play("IceSlide");
                            break;
                        }
                        else {
                            noIce = true;
                        }
                    }
                }
            }
        }
        else {
            while (currentGround.GetTile(currentDestination).name.Contains("Ice")) {
                currentDestination = currentDestination + direction;
                onIce = true;
                FindObjectOfType<AudioManager>().Play("IceSlide");
                if (currentGround.GetTile(currentDestination) == null) {
                    break;
                }
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
            FindObjectOfType<GMPlayer>().ResetLevel();
        }
    }
}
