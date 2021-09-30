using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator transition;

    public void ToStatueScene() {
        LoadLevel(GMPlayer.GetStatueScene());
    }
    public void ToPlayerScene() {
        if (GameObject.Find("Selected(Clone)") != null) {
            SelectionManager.selectedObject = null;
            SelectionManager.RemoveHighlight();
            SelectionManager.selected = false;
        }
        LoadLevel(GMStatue.GetPlayerScene());
    }
    public void LoadLevel(string levelName) {

        transition.SetTrigger("Start");
        SceneManager.LoadScene(levelName);

    }
}
