using UnityEngine;

public class Wall : MonoBehaviour {

    void Awake() {
        if (transform.childCount > 0) {
            if (GetComponentInChildren<SpriteRenderer>().sprite.name != "Ground Exit Tile") {
                GetComponentInChildren<Animator>().SetTrigger("IsTile");
            }
        }
    }
}
