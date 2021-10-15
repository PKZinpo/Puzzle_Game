using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator transition;

    public void ToStatueScene() {
        StartCoroutine(LoadLevel(GMPlayer.GetStatueScene()));
    }
    public void ToPlayerScene() {
        if (GameObject.Find("Selected(Clone)") != null) {
            SelectionManager.selectedObject = null;
            SelectionManager.RemoveHighlight();
            SelectionManager.selected = false;
        }
        StartCoroutine(LoadLevel(GMStatue.GetPlayerScene()));
    }
    public void ToNextLevel(string nextlevelScene) {
        StartCoroutine(LoadLevel(nextlevelScene));
        StatueData.statueList.Clear();
        StatueData.statueUIList.Clear();
        GMStatue.ClearActivatorPositions();
        GMStatue.numStatue = 0;
    }
    private IEnumerator LoadLevel(string levelName) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelName);
    }
}
