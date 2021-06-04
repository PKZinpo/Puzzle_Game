using UnityEngine;
using System.Collections.Generic;

public class StatueIcon {
    public string type;
    public bool isTurned;
}

public class StatueData : MonoBehaviour {

    #region Variables

    public GameObject statueIconPrefab;
    public static GameObject statueIcon;
    public GameObject statueIconParent;
    public Canvas canvas;

    public static Dictionary<Vector3, StatueIcon> statueList = new Dictionary<Vector3, StatueIcon>();
    public static List<Sprite> iconSprites = new List<Sprite>();
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
        LoadIconDictionary();
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
            if (statueUIList.Count == 0) {
                foreach (var item in statueList) {
                    statueUIList.Add(item.Key);
                }
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

    private void LoadIconDictionary() {
        Sprite[] spritesData = Resources.LoadAll<Sprite>("Icons");
        for (int i = 0; i < spritesData.Length; i++) {
            iconSprites.Add(spritesData[i]);
        }
    }
}
