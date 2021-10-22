using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TitleCube : MonoBehaviour {

    public Tile brokenTile;
    public Tilemap titleMap;
    public GameObject brokenTileObject;

    private int moveVal = 0;
    private int brokenVal = 0;
    private float movementSpeed = 1f;
    private float moveTime = 1.5f;
    private Vector3 destination;
    private Vector3Int prevgridPos;
    private bool moving = false;

    void Start() {
        prevgridPos = titleMap.WorldToCell(transform.position);
        if (transform.position != titleMap.CellToWorld(titleMap.WorldToCell(transform.position))) {
            transform.position = titleMap.CellToWorld(titleMap.WorldToCell(transform.position));
        }
        StartCoroutine(MoveTitleCube());
        StartCoroutine(MoveBrokenTile());
    }

    void Update() {
        if (moving) {
            transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
            if (transform.position == destination) {
                moving = false;
                DropBrokenTile();
            }
        }
    }

    private IEnumerator MoveTitleCube() {
        yield return new WaitForSeconds(moveTime);
        Vector3Int selectedgridPos;
        Vector3Int togridPos;
        switch (moveVal) {
            case 0:
                selectedgridPos = titleMap.WorldToCell(transform.position);
                togridPos = new Vector3Int(selectedgridPos.x - 1, selectedgridPos.y, 0);
                destination = titleMap.CellToWorld(togridPos);
                moving = true;
                moveVal++;
                break;

            case 1:
                selectedgridPos = titleMap.WorldToCell(transform.position);
                togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y - 1, 0);
                destination = titleMap.CellToWorld(togridPos);
                moving = true;
                moveVal++;
                break;

            case 2:
                selectedgridPos = titleMap.WorldToCell(transform.position);
                togridPos = new Vector3Int(selectedgridPos.x + 1, selectedgridPos.y, 0);
                destination = titleMap.CellToWorld(togridPos);
                moving = true;
                moveVal++;
                break;

            case 3:
                selectedgridPos = titleMap.WorldToCell(transform.position);
                togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y + 1, 0);
                destination = titleMap.CellToWorld(togridPos);
                moving = true;
                moveVal = 0;
                break;

        }
        StartCoroutine(MoveTitleCube());
    }

    private IEnumerator MoveBrokenTile() {
        yield return new WaitForSeconds(moveTime);
        Vector3Int brokenPos;
        switch (brokenVal) {
            case 0:
                brokenPos = new Vector3Int(0, -1, 0);
                if (!titleMap.HasTile(brokenPos)) {
                    GameObject tile = Instantiate(brokenTileObject);
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                    tile.transform.position = titleMap.CellToWorld(brokenPos);
                }
                brokenVal++;
                break;

            case 1:
                brokenPos = new Vector3Int(0, 0, 0);
                if (!titleMap.HasTile(brokenPos)) {
                    GameObject tile = Instantiate(brokenTileObject);
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                    tile.transform.position = titleMap.CellToWorld(brokenPos);
                }
                brokenVal++;
                break;

            case 2:
                brokenPos = new Vector3Int(-1, 0, 0);
                if (!titleMap.HasTile(brokenPos)) {
                    GameObject tile = Instantiate(brokenTileObject);
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                    tile.transform.position = titleMap.CellToWorld(brokenPos);
                }
                brokenVal++;
                break;

            case 3:
                brokenPos = new Vector3Int(-1, -1, 0);
                if (!titleMap.HasTile(brokenPos)) {
                    GameObject tile = Instantiate(brokenTileObject);
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                    tile.transform.position = titleMap.CellToWorld(brokenPos);
                }
                brokenVal = 0;
                break;

        }
        StartCoroutine(MoveBrokenTile());
    }
    private void DropBrokenTile() {
        titleMap.SetTile(prevgridPos, null);
        GameObject temp = Instantiate(brokenTileObject);
        temp.transform.position = titleMap.CellToWorld(prevgridPos);
        temp.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
        temp.GetComponent<TileProperties>().ChangeMoveUp();
        temp.GetComponent<TileProperties>().ChangeDisappearing();
        prevgridPos = titleMap.WorldToCell(transform.position);
    }
    public void PlaceTile(Vector3 position) {
        titleMap.SetTile(titleMap.WorldToCell(position), brokenTile);
    }
}
