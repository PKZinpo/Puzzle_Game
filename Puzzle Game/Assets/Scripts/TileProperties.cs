using UnityEngine;
using UnityEngine.Rendering;

public class TileProperties : MonoBehaviour {

    public Animator anim;

    public bool moveUp = false;
    public bool disappearing = false;
    public string animationName;

    void Start() {
        animationName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (name.Contains("Ground")) {
            moveUp = true;
        }
    }

    void Update() {
        if (name.Contains("Broken")) {
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
