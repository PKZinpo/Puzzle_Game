using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GMPlayer : MonoBehaviour {
    // Start is called before the first frame update

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
        stepVal = 0;
        highlightVal = -1;
    }

    void Start() {
        #region Level1

        if (currentMap.name == "lvl1Ground") {
            lvl1tilePlace = GMStatue.LVL1activator();
            for (int i = 0; i < 3; i++) {
                tilePositions[i] = new Vector3Int(lvl1tilePlace.x, lvl1tilePlace.y + 1 - i, 0);
            }
        }
        #endregion
    }

    void Update() {

        #region Level1

        if (currentMap.name == "lvl1Ground") {
            if (lvl1Path && !currentMap.HasTile(tilePositions[0])) {
                for (int i = 0; i < tilePositions.Length; i++) {
                    if (!currentMap.HasTile(tilePositions[i])) {
                        currentMap.SetTile(tilePositions[i], groundTile);
                    }
                }
            }
        }

        #endregion


        if (StatueData.statueUIList.Count > 0) {
            if (highlightVal != stepVal) {
                if (GameObject.Find("TileHighlight(Clone)")) {
                    foreach (var tile in GameObject.FindGameObjectsWithTag("TileHighlight")) {
                        Destroy(tile);
                    }
                }
                if (stepVal != StatueData.statueUIList.Count) {
                    AddTileHighlight();
                }
                highlightVal = stepVal;
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
    private void ToNextLevel() {
        SceneManager.LoadScene(nextlevelScene);
        StatueData.statueList.Clear();
    }
    public void ResetLevel() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void NextStep() {
        if (stepVal > StatueData.statueUIList.Count - 1) {
            Debug.Log("All Statues Have Been Activated");
        }
        else {
            foreach (var item in StatueData.statueList) {
                if (StatueData.statueUIList[stepVal] == item.Key) {
                    ChooseStatue(item.Value.type);
                    stepVal++;
                    break;
                }
            }
        }
    }
    public void ChooseStatue(string type) {
        switch (type) {
            case "3Line":
                PlaceTile(ThreeLine());
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
    public void PlaceTile(Vector3Int[] tilePlace) {
        for (int i = 0; i < tilePlace.Length; i++) {
            var wallPos = new Vector3Int(tilePlace[i].x + 1, tilePlace[i].y + 1, 0);
            if (currentMap.HasTile(tilePlace[i]) && !currentWall.HasTile(wallPos)) {
                currentWall.SetTile(wallPos, wallTile);
                PlayerMovement.MovePlayerUp(tilePlace[i], wallPos);
                Debug.Log("Placed Wall");
            }
            else if (!currentMap.HasTile(tilePlace[i])) {
                currentMap.SetTile(tilePlace[i], groundTile);
                Debug.Log("Placed Ground");
            }
            else {
                currentWall.SetTile(wallPos, null);
                currentMap.SetTile(tilePlace[i], null);
                Debug.Log("Removed Wall and Ground");
            }
        }
    }
    private void AddTileHighlight() {
        foreach (var item in StatueData.statueList) {
            if (StatueData.statueUIList[stepVal] == item.Key) {
                switch (item.Value.type) {
                    case "3Line":
                        for (int i = 0; i < ThreeLine().Length; i++) {
                            tileHighlight = Instantiate(tileHighlightPrefab);
                            tileHighlight.transform.position = currentMap.CellToWorld(ThreeLine()[i]);
                        }
                        break;

                    case "Ice3Line":
                        IceThreeLine();
                        break;
                }
            }
        }
    }
}
