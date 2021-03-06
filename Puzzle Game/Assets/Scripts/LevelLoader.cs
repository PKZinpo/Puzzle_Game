using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator transition;

    void Awake() {
        //if (!SceneManager.GetActiveScene().name.Contains("Title")) {
        LoadGame();
        FindObjectOfType<GameObjectData>().CheckLevel();
        //}
        Time.timeScale = 1f;
        if (!SceneManager.GetActiveScene().name.Contains("Title") && !SceneManager.GetActiveScene().name.Contains("Level")) {
            if (SceneManager.GetActiveScene().name.Contains("Statue")) {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().TrackTwoChange(false);
            }
            else {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().TrackTwoChange(true);
            }
        }
    }
    private void Start() {
        if (SceneManager.GetActiveScene().name.Contains("Title") || SceneManager.GetActiveScene().name.Contains("Level")) {
            if (FindObjectOfType<GameObjectData>().soundOff) {
                GameObject.Find("Canvas").transform.Find("Sound").gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.Find("SoundOff").gameObject.SetActive(true);
                FindObjectOfType<AudioManager>().StartMuteSound();
                //Debug.Log("Sound is off");
            }
            if (FindObjectOfType<GameObjectData>().musicOff) {
                GameObject.Find("Canvas").transform.Find("Music").gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.Find("MusicOff").gameObject.SetActive(true);
                FindObjectOfType<AudioManager>().StartMuteMusic();
                //Debug.Log("Music is off");
            }
        }
    }
    public void ToStatueScene() {
        StartCoroutine(LoadLevel(GMPlayer.GetStatueScene()));
        SaveGame();
    }
    public void ToPlayerScene() {
        if (GameObject.Find("Selected(Clone)") != null) {
            SelectionManager.selectedObject = null;
            SelectionManager.RemoveHighlight();
            SelectionManager.selected = false;
        }
        StartCoroutine(LoadLevel(GMStatue.GetPlayerScene()));
        SaveGame();
    }
    public void ToNextLevel(string nextlevelScene) {
        FindObjectOfType<AudioManager>().Play("NextLevel");
        StatueData.statueList.Clear();
        StatueData.statueUIList.Clear();
        GMStatue.ClearActivatorPositions();
        GMStatue.numStatue = 0;
        GameObject.Find("AudioManager").GetComponent<AudioManager>().toNextLevel = true;
        StartCoroutine(LoadLevel(nextlevelScene));
        SaveGame();
    }
    public void ToSelectedLevel(string level) {
        //GameObject.Find("Canvas").transform.Find("Level List").GetComponent<LevelList>().VolumeOff();
        GameObject.Find("AudioManager").GetComponent<AudioManager>().TitleVolumeOff();
        GameObject.Find("AudioManager").GetComponent<AudioManager>().ChangeTitleScreen(false);
        StartCoroutine(LoadLevel(level));
        SaveGame();
    }
    public void ToLevelSelection() {
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        GameObject.Find("AudioManager").GetComponent<AudioManager>().MainTrackVolumeOff();
        GameObject.Find("AudioManager").GetComponent<AudioManager>().ChangeTitleScreen(true);
        StartCoroutine(LoadLevel("Level Screen"));
        SaveGame();
    }
    private IEnumerator LoadLevel(string levelName) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelName);
    }
    public void SaveGame() {
        SaveSystem.SaveGameData(FindObjectOfType<GameObjectData>());
        Debug.Log("SaveGame");
    }
    public void LoadGame() {
        
        GameData data = SaveSystem.LoadGameData();
        GameObjectData game = FindObjectOfType<GameObjectData>();
        if (data == null) {
            game.currentLevel = 1;
            game.lvl1Tutorial = false;
            game.lvl1StatueTutorial = false;
            game.lvl2Tutorial = false;
            game.lvl2StatueTutorial = false;
            game.lvl3Tutorial = false;
            game.lvl4Tutorial = false;
            game.lvl5Tutorial = false;
            game.lvl6Tutorial = false;
            game.lvl7Tutorial = false;
            game.soundOff = false;
            game.musicOff = false;
            Debug.Log("NewGame");
            return;
        }
        
        game.currentLevel = data.level;
        game.lvl1Tutorial = data.lvl1Tutorial;
        game.lvl1StatueTutorial = data.lvl1StatueTutorial;
        game.lvl2Tutorial = data.lvl2Tutorial;
        game.lvl2StatueTutorial = data.lvl2StatueTutorial;
        game.lvl3Tutorial = data.lvl3Tutorial;
        game.lvl4Tutorial = data.lvl4Tutorial;
        game.lvl5Tutorial = data.lvl5Tutorial;
        game.lvl6Tutorial = data.lvl6Tutorial;
        game.lvl7Tutorial = data.lvl7Tutorial;
        game.soundOff = data.soundOff;
        game.musicOff = data.musicOff;
        Debug.Log("LoadGame");
    }
    public void ToTitleScreen() {
        StartCoroutine(LoadLevel("Title Screen"));
        SaveGame();
    }
    public void ExitGame() {
        Application.Quit();
    }
    public void OnApplicationQuit() {
        if (!SceneManager.GetActiveScene().name.Contains("Title") && !SceneManager.GetActiveScene().name.Contains("Level")) {
            SaveGame();
        }
    }
    public void ResetLevels() {
        GameObjectData game = FindObjectOfType<GameObjectData>();

        game.currentLevel = 1;
        game.lvl1Tutorial = false;
        game.lvl1StatueTutorial = false;
        game.lvl2Tutorial = false;
        game.lvl2StatueTutorial = false;
        game.lvl3Tutorial = false;
        game.lvl4Tutorial = false;
        game.lvl5Tutorial = false;
        game.lvl6Tutorial = false;
        game.lvl7Tutorial = false;

        SaveSystem.SaveGameData(FindObjectOfType<GameObjectData>());
        GameObject.Find("Canvas").transform.Find("Level List").GetComponent<LevelList>().UpdateLevels();
        Debug.Log("Levels Reset");
    }
    public void ToMuteSound(bool toOff) {
        FindObjectOfType<AudioManager>().MuteSound(toOff);
    }
    public void ToMuteMusic(bool toOff) {
        FindObjectOfType<AudioManager>().MuteMusic(toOff);
    }
}
