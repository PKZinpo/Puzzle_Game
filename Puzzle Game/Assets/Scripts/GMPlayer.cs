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
    public GameObject tileHighlightPrefab;
    public GameObject shadowPrefab;
    public Tilemap currentMap;
    public bool nextlevelOnWall;

    public static int highlightVal;
    public static int stepVal;
    public static GameObject tileHighlight;
    public static GameObject shadow;

    private bool hideWall = false;
    private Vector3Int direction;

    private static string sceneSwitchTo;
    private static Vector3 nextlevelOffset;

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

        if (nextlevelOnWall) {
            nextlevelOffset = new Vector3(0.0f, -0.045f, 0.0f);
        }
        else {
            nextlevelOffset = new Vector3(0.0f, -0.005f, 0.0f);
        }
        if (nextLevel.transform.position != currentMap.CellToWorld(currentMap.WorldToCell(nextLevel.transform.position))) {
            Vector3 newPos = new Vector3(nextLevel.transform.position.x, nextLevel.transform.position.y, 0);
            nextLevel.transform.position = currentMap.CellToWorld(currentMap.WorldToCell(newPos)) + nextlevelOffset;
            var childOffset = nextlevelOffset;
            if (nextlevelOnWall) {
                childOffset = new Vector3(nextlevelOffset.x, nextlevelOffset.y - 0.16f, nextlevelOffset.z);
            }
            nextLevel.transform.GetChild(0).transform.position += new Vector3(childOffset.x, Mathf.Abs(childOffset.y), childOffset.z);
        }
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

        if (hideWall) {
            if (GameObject.FindGameObjectsWithTag("Tile").Length != 0) {
                foreach (var wall in GameObject.FindGameObjectsWithTag("Tile")) {
                    Color color = wall.GetComponent<SpriteRenderer>().color;
                    if (color.a >= 0) {
                        color.a -= 0.01f;
                        wall.GetComponent<SpriteRenderer>().color = color;
                    }
                }
            }
            GameObject.FindGameObjectWithTag("NextLevel").GetComponentInChildren<Animator>().SetTrigger("HideOn");
        }
        else if (!hideWall) {
            if (GameObject.FindGameObjectsWithTag("Tile").Length != 0) {
                foreach (var wall in GameObject.FindGameObjectsWithTag("Tile")) {
                    Color color = wall.GetComponent<SpriteRenderer>().color;
                    if (color.a <= 1) {
                        color.a += 0.01f;
                        wall.GetComponent<SpriteRenderer>().color = color;
                    }
                }
            }
            GameObject.FindGameObjectWithTag("NextLevel").GetComponentInChildren<Animator>().SetTrigger("HideOff");
        }

        if (GameObject.FindGameObjectWithTag("NextLevel")) {
            if (currentMap.WorldToCell(playerObject.transform.position) == currentMap.WorldToCell(nextLevel.transform.GetChild(0).transform.position)) {
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
                            bool isWall = false;
                            foreach (var wall in GameObject.FindGameObjectsWithTag("Wall")) {
                                if (wall.transform.position == currentMap.CellToWorld(ThreeLine()[i]) + TileMoving.wallOffset) {
                                    isWall = true;
                                }
                            }
                            if (isWall) {
                                tileHighlight.transform.position = currentMap.CellToWorld(wallCheck);
                                tileHighlight.GetComponent<SpriteRenderer>().sortingLayerName = "1st Floor";
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
    public void HideWallTiles() {
        hideWall = !hideWall;
    }
}
