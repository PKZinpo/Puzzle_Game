using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour {

    void Start() {
        StartCoroutine(CreditsEnd());
    }

    private IEnumerator CreditsEnd() {
        yield return new WaitForSeconds(5f);
        FindObjectOfType<LevelLoader>().ToLevelSelection();
    }
}
