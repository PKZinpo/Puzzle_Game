using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GMPlayer : MonoBehaviour {

    #region GeneralVariables

    public float movementSpeed = 1f;
    public string statueScene;
    public string nextlevelScene;
    public GameObject nextLevel;
    public GameObject playerObject;
    public Tilemap currentMap;
    public Tilemap currentWall;
    public Tile groundTile;
    public Tile wallTile;

    public static int highlightVal;
    public static int stepVal;
    private Vector3Int direction;
    private static string sceneSwitchTo;

    public GameObject tileHighlightPrefab;
    public static GameObject tileHighlight;
    public GameObject shadowPrefab;
    public static GameObject shadow;
    

    #endregion

    #region Level1Variables

    private static Vector3Int[] tilePositions = new Vector3Int[3];
    
    public static bool lvl1Path = false;
    private Vector3Int lvl1tilePlace;

    #endregion

    void Awake() {
        sceneSwitchTo = statueScene;
        stepVal = 0;
        highlightVal = -1;
    }

    void Start() {
        #region Level1

        #endregion
    }

    void Update() {

        #region Level1


        #endregion

        if (StatueData.statueUIList.Count > 0) {
            if (!TileMoving.isMoving) {
                if (highlightVal != stepVal) {
                    if (stepVal != StatueData.statueUIList.Count) {
                        AddTileHighlight();
                    }
                    highlightVal = stepVal;
                }
            }
        }

        if (GameObject.Find("NextLevel")) {
            if (currentMap.WorldToCell(playerObject.transform.position) == currentMap.WorldToCell(nextLevel.transform.position)) {
                ToNextLevel();
            }
        }
    }

    public void ToStatueScene() {
        SceneManager.LoadScene(statueScene);
    }
    public static string GetStatueScene() {
        return sceneSwitchTo;
    }
    public void NextStep() {
        if (!TileMoving.isMoving) {
            if (stepVal > StatueData.statueUIList.Count - 1) {
                Debug.Log("All Statues Have Been Activated");
            }
            else {
                if (GameObject.Find("TileHighlight(Clone)")) {
                    foreach (var tile in GameObject.FindGameObjectsWithTag("TileHighlight")) {
                        Destroy(tile);
                    }
                }
                foreach (var item in StatueData.statueList) {
                    if (StatueData.statueUIList[stepVal] == item.Key) {
                        ChooseType(item.Value.type);
                        stepVal++;
                        break;
                    }
                }
            }
        }
    }
    public void ChooseType(string type) {
        switch (type) {
            case "3Line":
                TileMoving.MoveTiles(ThreeLine());
                break;

            case "Ice3Line":
                IceThreeLine();
                break;
        }
    }
    public Vector3Int[] ThreeLine() {
        Vector3Int[] tilePlaceArray = new Vector3Int[3];
        var statue = StatueData.statueUIList[stepVal];
        foreach (var item in StatueData.statueList) {
            if (statue == item.Key) {
                var statueGridPos = currentMap.WorldToCell(item.Key);
                direction = (Convert.ToInt32(item.Value.isTurned) * Vector3Int.up) + (Convert.ToInt32(!item.Value.isTurned) * Vector3Int.right);
                for (int i = 0; i < 3; i++) {
                    var tilePlace = statueGridPos + (i - 1) * direction;
                    tilePlaceArray[i] = tilePlace;
                }
            }
        }
        return tilePlaceArray;
    }
    public void IceThreeLine() {
        Debug.Log("Ice3Line");


    }
    private void AddTileHighlight() {
        foreach (var item in StatueData.statueList) {
            if (StatueData.statueUIList[stepVal] == item.Key) {
                switch (item.Value.type) {
                    case "3Line":
                        for (int i = 0; i < ThreeLine().Length; i++) {
                            tileHighlight = Instantiate(tileHighlightPrefab);
                            var wallCheck = new Vector3Int(ThreeLine()[i].x + 1, ThreeLine()[i].y + 1, 0);
                            if (currentWall.HasTile(wallCheck)) {
                                tileHighlight.transform.position = currentWall.CellToWorld(wallCheck);
                                tileHighlight.GetComponent<SpriteRenderer>().sortingLayerName = "1st Floor Shadow";
                            }
                            else {
                                tileHighlight.transform.position = currentMap.CellToWorld(ThreeLine()[i]);
                            }
                            
                        }
                        break;

                    case "Ice3Line":
                        IceThreeLine();
                        break;
                }
            }
        }
    }
    public static void ResetLevel() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    private void ToNextLevel() {
        SceneManager.LoadScene(nextlevelScene);
        StatueData.statueList.Clear();
        StatueData.statueUIList.Clear();
        GMStatue.ClearActivatorPositions();
        GMStatue.numStatue = 0;
    }
    
}
