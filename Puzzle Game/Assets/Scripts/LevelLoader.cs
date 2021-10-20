using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator transition;

    void Awake() {
        if (SceneManager.GetActiveScene().name.Contains("Player")) {
            LoadGame();
            Debug.Log("LoadGame");
        }
        Time.timeScale = 1f;
    }

    public void ToStatueScene() {
        StartCoroutine(LoadLevel(GMPlayer.GetStatueScene()));
        SaveGame();
        Debug.Log("SaveGame");
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
        StatueData.statueList.Clear();
        StatueData.statueUIList.Clear();
        GMStatue.ClearActivatorPositions();
        GMStatue.numStatue = 0;
        StartCoroutine(LoadLevel(nextlevelScene));
        SaveGame();
        Debug.Log("SaveGame");
    }
    public void ToSelectedLevel(string level) {
        if (GameObject.Find("Selected(Clone)") != null) {
            SelectionManager.selectedObject = null;
            SelectionManager.RemoveHighlight();
            SelectionManager.selected = false;
        }
        StatueData.statueList.Clear();
        StatueData.statueUIList.Clear();
        GMStatue.ClearActivatorPositions();
        GMStatue.numStatue = 0;
        StartCoroutine(LoadLevel(level));
    }
    public void ToLevelSelection() {
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        StartCoroutine(LoadLevel("Level Screen"));
    }
    private IEnumerator LoadLevel(string levelName) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelName);
    }
    public void SaveGame() {
        SaveSystem.SaveGameData(FindObjectOfType<GMPlayer>());
    }
    public void LoadGame() {
        GameData data = SaveSystem.LoadGameData();

        //GMPlayer player = FindObjectOfType<GMPlayer>();
        //player.currentLevel = data.level;
        //player.lvl1Tutorial = data.lvl1Tutorial;
        //player.lvl2Tutorial = data.lvl2Tutorial;
        //player.lvl3Tutorial = data.lvl3Tutorial;
        //player.lvl4Tutorial = data.lvl4Tutorial;
        //player.lvl5Tutorial = data.lvl5Tutorial;
        //player.lvl6Tutorial = data.lvl6Tutorial;
        //player.lvl7Tutorial = data.lvl7Tutorial;

        Debug.Log(data.level);
        Debug.Log(data.lvl1Tutorial);
        Debug.Log(data.lvl2Tutorial);
        Debug.Log(data.lvl3Tutorial);
        Debug.Log(data.lvl4Tutorial);
        Debug.Log(data.lvl5Tutorial);
        Debug.Log(data.lvl6Tutorial);
        Debug.Log(data.lvl7Tutorial);
    }
}
