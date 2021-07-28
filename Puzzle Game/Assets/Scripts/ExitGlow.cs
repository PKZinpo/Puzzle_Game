using UnityEngine;

public class ExitGlow : MonoBehaviour {

    public Animator exitGlow;

    private void SwitchGlow() {
        exitGlow.SetTrigger("Switch Glow");
    }
}
