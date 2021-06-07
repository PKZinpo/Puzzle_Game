using UnityEngine;

public class StatueType : MonoBehaviour {

    #region Variables

    public Animator anim;

    public string statueType;

    public bool yAxis = false;
    private bool turn = false;

    #endregion

    void Update() {
        anim.SetBool("Turn", turn);
        anim.SetBool("yAxis", yAxis);
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
}
