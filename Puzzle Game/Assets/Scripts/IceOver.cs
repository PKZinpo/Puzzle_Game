using UnityEngine;
using UnityEngine.Tilemaps;

public class IceOver : MonoBehaviour {

    public Tile brokenFloor;
    public Tile groundHalfFloor;
    public GameObject groundHalfWall;
    public GameObject brokenWall;

    private Tilemap map = GMPlayer.currentMapStatic;

    private void GroundHalfWall() {
        
    }
    private void BrokenWall() {

    }
    private void GroundHalfFloor() {
        map.SetTile(map.WorldToCell(transform.position), null);
        map.SetTile(map.WorldToCell(transform.position), groundHalfFloor);
        Destroy(gameObject);
    }
    private void BrokenFloor() {
        map.SetTile(map.WorldToCell(transform.position), null);
        map.SetTile(map.WorldToCell(transform.position), brokenFloor);
        Destroy(gameObject);
    }
}
