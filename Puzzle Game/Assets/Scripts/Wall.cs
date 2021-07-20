using UnityEngine;

public class Wall : MonoBehaviour {

    void Awake() {
        if (transform.childCount > 0) {
            GetComponentInChildren<Animator>().SetTrigger("IsTile");
        }
    }
}
