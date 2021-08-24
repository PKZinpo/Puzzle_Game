using UnityEngine;

public class StatueType : MonoBehaviour {

    #region Variables

    public Animator anim;

    public string statueType;

    public bool yAxis = false;
    public bool onSwitch = false;
    public bool isOn = false;

    private bool turn = false;
    

    #endregion

    void Start() {
        if (isOn) {
            if (yAxis) {
                anim.SetTrigger("StartOnY");
            }
            else {
                anim.SetTrigger("StartOnX");
            }
        }
        if (yAxis) {
            anim.SetTrigger("StartOffY");
        }
    }

    void Update() {
        anim.SetBool("Turn", turn);
        anim.SetBool("yAxis", yAxis);

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
        turn = !turn;
        if (yAxis) {
            yAxis = !yAxis;
        }
        StatueData.PopulateStatueList();
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
    }
    public void IdleToOff() {
        anim.SetBool("ToOff", false);
    }
}
