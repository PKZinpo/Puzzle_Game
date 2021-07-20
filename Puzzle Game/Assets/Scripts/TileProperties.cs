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
        //if (GetComponentInParent<SortingGroup>().sortingLayerName == "Ground") {
        //    anim.SetTrigger("IsTile");
        //}
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
            Debug.Log("Disappear");
            Destroy(gameObject.transform.parent.gameObject);
        }
        else if (moveUp) {
            TileMoving.PlaceTiles(transform.position, "Wall", name);
            Debug.Log("Move Up");
            Destroy(gameObject);
        }
        else {
            TileMoving.PlaceTiles(transform.position, "Ground", name);
            Debug.Log("Appear");
            Destroy(gameObject);
        }
    }
    public void StartTile() {
        TileMoving.PlaceTiles(transform.position, "Wall", name);
        Destroy(gameObject);
    }
}
