using UnityEngine;

public class Wall : MonoBehaviour {

    void Awake() {
        if (transform.childCount > 0) {
            if (!GetComponentInChildren<SpriteRenderer>().sprite.name.Contains("Exit")) {
                GetComponentInChildren<Animator>().SetTrigger("IsTile");
            }
        }
    }
}
