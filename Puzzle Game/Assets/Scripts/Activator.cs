using UnityEngine;

public class Activator : MonoBehaviour {

    private Animator anim;
    private bool activatorOn = false;

    void Awake() {
        anim = GetComponent<Animator>();
    }
    public void SwitchActivator() {
        activatorOn = !activatorOn;
        if (!activatorOn) {
            anim.SetBool("Activator", activatorOn);
        }
    }
}
