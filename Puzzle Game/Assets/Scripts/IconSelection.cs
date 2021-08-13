using System.Collections;
using UnityEngine;

public class IconSelection : MonoBehaviour {

    public Animator shineSelection;

    private bool shine = false;

    void Start() {
        StartCoroutine(ShineTimer());
    }

    private void SwitchShine() {
        shineSelection.SetBool("Shine", shine);
    }
    private void SwitchBool() {
        shine = !shine;
        SwitchShine();
        if (!shine) {
            StartCoroutine(ShineTimer());
        }
    }
    IEnumerator ShineTimer() {
        yield return new WaitForSeconds(4f);
        SwitchBool();
    }
}
