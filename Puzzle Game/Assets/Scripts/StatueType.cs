using UnityEngine;

public class StatueType : MonoBehaviour {

    #region Variables

    public Animator anim;

    public string statueType;

    public bool yAxis = false;
    public bool onSwitch = false;
    public bool isOn = false;
    public bool isPlaced = false;

    private bool turn = false;
    

    #endregion

    void Start() {
        if (isOn) {
            if (statueType.Contains("Ice")) {
                anim.SetTrigger("StartOn");
            }
            else {
                if (yAxis) {
                    anim.SetTrigger("StartOnY");
                }
                else {
                    anim.SetTrigger("StartOnX");
                }
            }
        }
        else if (yAxis) {
            anim.SetTrigger("StartOffY");
        }
    }

    void Update() {
        if (!statueType.Contains("Ice")) {
            anim.SetBool("Turn", turn);
            anim.SetBool("yAxis", yAxis);
        }
        if (onSwitch) {
            if (!isOn) {
                isOn = true;
                anim.SetBool("ToOn", true);
            }
            else {
                isOn = false;
                anim.SetBool("ToOff", true);
            }
            onSwitch = false;
        }
    }

    public void ToTurn() {
        if (!statueType.Contains("Ice")) {
            turn = !turn;
            if (yAxis) {
                yAxis = !yAxis;
            }
            StatueData.PopulateStatueList();
        }
    }
    public void SwitchAxis() {
        yAxis = !yAxis;
        turn = !turn;
        StatueData.PopulateStatueList();
    }
    public string GetStatueType() {
        return statueType;
    }
    public void IdleToOn() {
        anim.SetBool("ToOn", false);
        StatueData.PopulateStatueList();
    }
    public void IdleToOff() {
        anim.SetBool("ToOff", false);
        StatueData.PopulateStatueList();
    }
}
