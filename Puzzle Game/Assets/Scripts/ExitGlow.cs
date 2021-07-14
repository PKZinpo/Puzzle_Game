using UnityEngine;

public class ExitGlow : MonoBehaviour {

    public Animator exitGlow;

    void Start() {
        
    }

    void Update() {
        
    }
    private void SwitchGlow() {
        exitGlow.SetTrigger("Switch Glow");
    }
}
