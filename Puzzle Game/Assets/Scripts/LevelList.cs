using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelList : MonoBehaviour {

    void Start() {
        for (int i = 1; i < transform.childCount; i++) {    
            string levelName = transform.GetChild(i).name;
            char val = levelName.ToCharArray()[levelName.ToCharArray().Length - 1];
            int level = Int32.Parse(val.ToString());
            if (level > FindObjectOfType<GameObjectData>().currentLevel) {
                transform.GetChild(i).GetComponent<Button>().interactable = false;
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
}
