using UnityEngine;
using System.Collections.Generic;

public class StatueIcon {
    public string type;
    public string animationName;
    public bool isTurned;
}

public class StatueData : MonoBehaviour {

    #region Variables

    public static Dictionary<Vector3, StatueIcon> statueList = new Dictionary<Vector3, StatueIcon>();
    public static List<Sprite> iconSprites = new List<Sprite>();
    public static List<Sprite> tileSprites = new List<Sprite>();
    public static List<Vector3> statueUIList = new List<Vector3>();
    private static GameObject[] Statues;
    private static StatueData positionObject;

    #endregion

    void Awake() {
        if (positionObject != null) {
            Destroy(gameObject);
            return;
        }
        positionObject = this;
        DontDestroyOnLoad(gameObject);
        LoadAllResources();
    }

    public static void PopulateStatueList() {
        if (GameObject.FindGameObjectsWithTag("Statue").Length != 0) {
            statueList.Clear();
            Statues = GameObject.FindGameObjectsWithTag("Statue");
            for (int i = 0; i < Statues.Length; i++) {
                statueList.Add(Statues[i].transform.position, new StatueIcon() {
                    type = Statues[i].GetComponent<StatueType>().statueType,
                    isTurned = Statues[i].GetComponent<StatueType>().yAxis
                });
            }
        }
    }
    public static Vector3[] GetStatuePositions() {
        Vector3[] tempPos = new Vector3[statueList.Count];
        int i = 0;
        foreach (var item in statueList) {
            tempPos[i] = item.Key;
            i++;
        }
        return tempPos;
    }
    private void LoadAllResources() {
        Sprite[] iconData = Resources.LoadAll<Sprite>("Icons");
        for (int i = 0; i < iconData.Length; i++) {
            iconSprites.Add(iconData[i]);
        }
        Sprite[] tileData = Resources.LoadAll<Sprite>("PuzzleGameTileSheet");
        for (int i = 0; i < tileData.Length; i++) {
            tileSprites.Add(tileData[i]);
        }
    }
}
