using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectData : MonoBehaviour {

    public int currentLevel;
    public bool lvl1Tutorial = false;
    public bool lvl1StatueTutorial = false;
    public bool lvl2Tutorial = false;
    public bool lvl2StatueTutorial = false;
    public bool lvl3Tutorial = false;
    public bool lvl4Tutorial = false;
    public bool lvl5Tutorial = false;
    public bool lvl6Tutorial = false;
    public bool lvl7Tutorial = false;

    void Start() {
        bool levelCheck = SceneManager.GetActiveScene().name.Contains("Player") || SceneManager.GetActiveScene().name.Contains("Statue");
        if (levelCheck) {
            string sceneName = SceneManager.GetActiveScene().name;
            char val = sceneName.ToCharArray()[sceneName.ToCharArray().Length - 1];
            currentLevel = Int32.Parse(val.ToString());
        }
    }
}
