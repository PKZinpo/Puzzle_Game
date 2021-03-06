using UnityEngine;
using UnityEngine.Tilemaps;

public class IceOver : MonoBehaviour {

    public Tile brokenFloor;
    public Tile groundHalfFloor;
    public GameObject groundHalfWall;
    public GameObject brokenWall;

    private Tilemap map = GMPlayer.currentMapStatic;
    private GameObject iceTile = null;

    private void GroundHalfWall() {
        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
            if (transform.position == tile.transform.position) {
                iceTile = Instantiate(groundHalfWall);
                iceTile.GetComponent<Animator>().SetTrigger("IsTile");
                iceTile.transform.parent = tile.transform.parent;
                iceTile.transform.localPosition = tile.transform.localPosition;
                Destroy(tile);
                break;
            }
        }
        Destroy(gameObject);
        IcingOverSwitch();
    }
    private void BrokenWall() {
        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
            if (transform.position == tile.transform.position) {
                iceTile = Instantiate(brokenWall);
                iceTile.GetComponent<Animator>().SetTrigger("IsTile");
                iceTile.transform.parent = tile.transform.parent;
                iceTile.transform.localPosition = tile.transform.localPosition;
                Destroy(tile);
                break;
            }
        }
        Destroy(gameObject);
        IcingOverSwitch();
    }
    private void GroundHalfFloor() {
        map.SetTile(map.WorldToCell(transform.position), null);
        map.SetTile(map.WorldToCell(transform.position), groundHalfFloor);
        Destroy(gameObject);
        IcingOverSwitch();
    }
    private void BrokenFloor() {
        map.SetTile(map.WorldToCell(transform.position), null);
        map.SetTile(map.WorldToCell(transform.position), brokenFloor);
        Destroy(gameObject);
        IcingOverSwitch();
    }
    private void IcingOverSwitch() {
        if (GameObject.Find("GMPlayer").GetComponent<GMPlayer>().isIcing) {
            GameObject.Find("GMPlayer").GetComponent<GMPlayer>().isIcing = false;
        }
        GameObject.Find("GMPlayer").GetComponent<GMPlayer>().EnableButtons();
    }
}
