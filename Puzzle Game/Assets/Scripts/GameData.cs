using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

    public int level;
    public bool lvl1Tutorial;
    public bool lvl2Tutorial;
    public bool lvl3Tutorial;
    public bool lvl4Tutorial;
    public bool lvl5Tutorial;
    public bool lvl6Tutorial;
    public bool lvl7Tutorial;

    public GameData (GMPlayer data) {

        level = data.currentLevel;
        lvl1Tutorial = data.lvl1Tutorial;
        lvl2Tutorial = data.lvl2Tutorial;
        lvl3Tutorial = data.lvl3Tutorial;
        lvl4Tutorial = data.lvl4Tutorial;
        lvl5Tutorial = data.lvl5Tutorial;
        lvl6Tutorial = data.lvl6Tutorial;
        lvl7Tutorial = data.lvl7Tutorial;

    }

}
