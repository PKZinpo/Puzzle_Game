using UnityEngine;
using UnityEngine.SceneManagement;

public class TileProperties : MonoBehaviour {

    public Animator anim;

    public bool moveUp = false;
    public bool disappearing = false;

    void Start() {
        if (name.Contains("Ground")) {
            moveUp = true;
        }
    }

    void Update() {
        if (name.Contains("Broken") && !name.Contains("Ice")) {
            anim.SetBool("Moving Up", moveUp);
        }
        anim.SetBool("Disappearing", disappearing);
        
    }
    public void ChangeMoveUp () {
        moveUp = true;
    }
    public void ChangeDisappearing() {
        disappearing = true;
    }
    public void RemoveTile() {
        if (disappearing) {
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
        else if (moveUp) {
            TileMoving.PlaceTiles(transform.position, "Wall", name);
            Destroy(gameObject);
        }
        else if (SceneManager.GetActiveScene().name.Contains("Title")) {
            FindObjectOfType<TitleCube>().PlaceTile(transform.position);
            Destroy(gameObject);
        }
        else {
            TileMoving.PlaceTiles(transform.position, "Ground", name);
            Destroy(gameObject);
        }
    }
    public void StartTile() {
        TileMoving.PlaceTiles(transform.position, "Wall", name);
        Destroy(gameObject);
    }
}
