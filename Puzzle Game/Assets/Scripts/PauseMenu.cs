using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuUI;

    public static bool isPaused = false;

    void Awake() {
        isPaused = false;
    }

    private void Start() {
        if (FindObjectOfType<GameObjectData>().soundOff) {
            PMSoundOff();
            FindObjectOfType<AudioManager>().StartMuteSound();
            //Debug.Log("Sound is off");
        }
        if (FindObjectOfType<GameObjectData>().musicOff) {
            PMMusicOff();
            FindObjectOfType<AudioManager>().StartMuteMusic();
            //Debug.Log("Music is off");
        }
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Exit() {
        if (GameObject.Find("Selected(Clone)") != null) {
            SelectionManager.selectedObject = null;
            SelectionManager.RemoveHighlight();
            SelectionManager.selected = false;
        }
        StatueData.statueList.Clear();
        StatueData.statueUIList.Clear();
        GMStatue.ClearActivatorPositions();
        GMStatue.numStatue = 0;
        FindObjectOfType<LevelLoader>().SaveGame();
        FindObjectOfType<LevelLoader>().ToLevelSelection();
        Debug.Log("EXIT");
    }
    public void PMToMuteSound(bool toOff) {
        FindObjectOfType<AudioManager>().MuteSound(toOff);
    }
    public void PMToMuteMusic(bool toOff) {
        FindObjectOfType<AudioManager>().MuteMusic(toOff);
    }
    public void PMSoundOff() {
        pauseMenuUI.transform.Find("Sound").gameObject.SetActive(false);
        pauseMenuUI.transform.Find("SoundOff").gameObject.SetActive(true);
    }
    public void PMMusicOff() {
        pauseMenuUI.transform.Find("Music").gameObject.SetActive(false);
        pauseMenuUI.transform.Find("MusicOff").gameObject.SetActive(true);
    }
}
