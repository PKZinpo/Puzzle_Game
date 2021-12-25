using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelList : MonoBehaviour {

    private bool toLevel = false;
    private AudioSource titleTrack;

    void Start() {
        for (int i = 1; i < transform.childCount; i++) {    
            string levelName = transform.GetChild(i).name;
            char val = levelName.ToCharArray()[levelName.ToCharArray().Length - 1];
            int level = Int32.Parse(val.ToString());
            if (level > FindObjectOfType<GameObjectData>().currentLevel) {
                transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
        for (int i = 0; i < FindObjectOfType<AudioManager>().GetComponents<AudioSource>().Length; i++) {
            if (FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i].clip.name == "Puzzle Game Track") {
                titleTrack = FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i];
            }
        }
    }

    private void Update() {
        if (toLevel) {
            if (titleTrack.volume >= 0f) {
                titleTrack.volume -= 1f * Time.deltaTime;
            }
        }
    }
    public void UpdateLevels() {
        for (int i = 1; i < transform.childCount; i++) {
            string levelName = transform.GetChild(i).name;
            char val = levelName.ToCharArray()[levelName.ToCharArray().Length - 1];
            int level = Int32.Parse(val.ToString());
            if (level > FindObjectOfType<GameObjectData>().currentLevel) {
                transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
    }
    public void VolumeOff() {
        toLevel = true;
    }
}
