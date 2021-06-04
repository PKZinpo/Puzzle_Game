using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GMStatue : MonoBehaviour {

    #region GeneralVariables

    public Tilemap currentMap;
    public float movementSpeed = 1f;
    public string playerScene;

    private Vector3Int togridPos;
    private Vector3Int selectedgridPos;
    private static GameObject moveObject;
    private static bool movetoDest = false;
    private static Vector3 destination;

    static private GameObject[] Statues;

    #endregion

    #region Level1Variables

    public GameObject lvl1Statue;
    public GameObject lvl1Activator;
    
    private static Vector3Int lvl1activatorgridPos;

    #endregion

    void Awake() {
        if (StatueData.GetStatuePositions() == null || StatueData.GetStatuePositions().Length < 1) {
            StatueData.PopulateStatueList();
        }
        if (GameObject.FindGameObjectsWithTag("Statue").Length != 0) {
            Statues = GameObject.FindGameObjectsWithTag("Statue");
            int i = 0;
            foreach (var item in StatueData.statueList) {
                Statues[i].transform.position = item.Key;
                Statues[i].GetComponent<StatueType>().statueType = item.Value.type;
                Statues[i].GetComponent<StatueType>().yAxis = item.Value.isTurned;
                i++;
            }
        }
    }

    void Start() {
        #region Level1

        if (currentMap.name == "lvl1Ground") {
            lvl1activatorgridPos = currentMap.WorldToCell(lvl1Activator.transform.position);
            lvl1activatorgridPos = new Vector3Int(lvl1activatorgridPos.x, lvl1activatorgridPos.y, 0);
        }

        #endregion
    }

    void Update() {

        #region Level1

        if (currentMap.name == "lvl1Ground") {
            if (currentMap.WorldToCell(lvl1Statue.transform.position) == lvl1activatorgridPos) {
                LVL1makePath();
            }
        }

        #endregion

        if (SelectionManager.selected) {
            moveObject = SelectionManager.objecttoMove;
            if (movetoDest) {
                moveObject.transform.position = Vector3.MoveTowards(moveObject.transform.position, destination, movementSpeed * Time.deltaTime);
                if (moveObject.transform.position == destination) {

                    #region Set New Position in Statue Array

                    StatueData.PopulateStatueList();
                    Vector3[] tempStatue = new Vector3[GameObject.FindGameObjectsWithTag("Statue").Length];
                    for (int i = 0; i < tempStatue.Length; i++) {
                        tempStatue[i] = GameObject.FindGameObjectsWithTag("Statue")[i].transform.position;
                    }
                    for (int i = 0; i < StatueData.statueUIList.Count; i++) {
                        if (!StatueData.statueList.ContainsKey(StatueData.statueUIList[i])) {
                            foreach (var item in tempStatue) {
                                if (!StatueData.statueUIList.Contains(item)) {
                                    StatueData.statueUIList[i] = item;
                                }
                            }
                            break;
                        }
                    }

                    #endregion

                    movetoDest = false;
                }
            }
        }

    }

    #region ArrowMovement
    public void movestatueTL() {
        if (SelectionManager.selected) {
            selectedgridPos = currentMap.WorldToCell(moveObject.transform.position);
            togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y + 1, 0);
            if (currentMap.HasTile(togridPos)) {
                destination = currentMap.CellToWorld(togridPos);
                destination = new Vector3(destination.x, destination.y, 0);
                movetoDest = true;
            }
        }
    }
    public void movestatueTR() {
        if (SelectionManager.selected) {
            selectedgridPos = currentMap.WorldToCell(moveObject.transform.position);
            togridPos = new Vector3Int(selectedgridPos.x + 1, selectedgridPos.y, 0);
            if (currentMap.HasTile(togridPos)) {
                destination = currentMap.CellToWorld(togridPos);
                destination = new Vector3(destination.x, destination.y, 0);
                movetoDest = true;
            }
        }
    }
    public void movestatueBR() {
        if (SelectionManager.selected) {
            selectedgridPos = currentMap.WorldToCell(moveObject.transform.position);
            togridPos = new Vector3Int(selectedgridPos.x, selectedgridPos.y - 1, 0);
            if (currentMap.HasTile(togridPos)) {
                destination = currentMap.CellToWorld(togridPos);
                destination = new Vector3(destination.x, destination.y, 0);
                movetoDest = true;
            }
        }
    }
    public void movestatueBL() {
        if (SelectionManager.selected) {
            selectedgridPos = currentMap.WorldToCell(moveObject.transform.position);
            togridPos = new Vector3Int(selectedgridPos.x - 1, selectedgridPos.y, 0);
            if (currentMap.HasTile(togridPos)) {
                destination = currentMap.CellToWorld(togridPos);
                destination = new Vector3(destination.x, destination.y, 0);
                movetoDest = true;
            }
        }
    }
    #endregion

    #region Level1

    public static Vector3Int LVL1activator() {
        return lvl1activatorgridPos;
    }

    private void LVL1makePath() {
        GMPlayer.lvl1Path = true;
    }

    #endregion

    public void ToPlayerScene() {
        if (GameObject.Find("Highlight(Clone)") != null) {
            SelectionManager.selectedObject = null;
            SelectionManager.RemoveHighlight();
            SelectionManager.selected = false;
        }
        SceneManager.LoadScene(playerScene);
    }
}
