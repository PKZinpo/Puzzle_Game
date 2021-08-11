using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GMStatue : MonoBehaviour {

    #region GeneralVariables

    public Tilemap currentMap;
    public float movementSpeed = 1f;
    public string playerScene;

    public static int numStatue;

    private Vector3Int togridPos;
    private Vector3Int selectedgridPos;
    private static GameObject moveObject;
    private static bool movetoDest = false;
    private static Vector3 destination;
    private static string sceneSwitchTo;

    private static GameObject[] Statues;
    private static List<Vector3> activatorPosition = new List<Vector3>();

    #endregion

    #region Level1Variables


    #endregion

    void Awake() {
        sceneSwitchTo = playerScene;

        foreach (var item in GameObject.FindGameObjectsWithTag("Statue")) {
            foreach (var position in activatorPosition) {
                if (item.transform.position == position) {
                    numStatue++;
                }
            }
        }

        #region Alignment

        if (GameObject.FindGameObjectsWithTag("Statue").Length != 0) {
            foreach (var statue in GameObject.FindGameObjectsWithTag("Statue")) {
                if (statue.transform.position != currentMap.CellToWorld(currentMap.WorldToCell(statue.transform.position))) {
                    statue.transform.position = currentMap.CellToWorld(currentMap.WorldToCell(statue.transform.position));
                }
            }
        }
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
        if (GameObject.FindGameObjectsWithTag("Activator").Length != 0) {
            foreach (var activator in GameObject.FindGameObjectsWithTag("Activator")) {
                if (activator.transform.position != currentMap.CellToWorld(currentMap.WorldToCell(activator.transform.position))) {
                    activator.transform.position = currentMap.CellToWorld(currentMap.WorldToCell(activator.transform.position));
                }
            }
            if (activatorPosition.Count == 0) {
                foreach (var item in GameObject.FindGameObjectsWithTag("Activator")) {
                    var pos = new Vector3(item.transform.position.x, item.transform.position.y, 0);
                    activatorPosition.Add(pos);
                }
            }
        }

        #endregion

    }

    void Start() {
        #region Level1

        

        #endregion
    }

    void Update() {

        #region Level1

        

        #endregion

        if (SelectionManager.selected) {
            moveObject = SelectionManager.objecttoMove;
            if (movetoDest) {
                if (StatueData.statueList.ContainsKey(destination)) {
                    movetoDest = false;
                }
                else {
                    moveObject.transform.position = Vector3.MoveTowards(moveObject.transform.position, destination, movementSpeed * Time.deltaTime);
                    if (moveObject.transform.position == destination) {

                        #region Set New Position in Statue Array

                        StatueData.PopulateStatueList();

                        List<Vector3> tempStatue = new List<Vector3>();
                        foreach (var item in GameObject.FindGameObjectsWithTag("Statue")) {
                            foreach (var position in activatorPosition) {
                                if (item.transform.position == position) {
                                    tempStatue.Add(position);

                                }
                            }
                        }
                        if (activatorPosition.Contains(moveObject.transform.position)) {
                            if (numStatue < tempStatue.Count) {
                                StatueData.statueUIList.Add(moveObject.transform.position);
                                numStatue++;
                                Debug.Log("Added position " + currentMap.WorldToCell(moveObject.transform.position));
                            }
                            else {
                                for (int i = 0; i < tempStatue.Count; i++) {
                                    if (!tempStatue.Contains(StatueData.statueUIList[i])) {
                                        for (int j = 0; j < tempStatue.Count; j++) {
                                            if (!StatueData.statueUIList.Contains(tempStatue[j])) {
                                                Debug.Log("Switched position " + currentMap.WorldToCell(StatueData.statueUIList[i]) + " to " + currentMap.WorldToCell(tempStatue[j]));
                                                StatueData.statueUIList[i] = tempStatue[j];

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            foreach (var item in StatueData.statueUIList) {
                                if (!tempStatue.Contains(item)) {
                                    Debug.Log("Removed position " + currentMap.WorldToCell(item));
                                    StatueData.statueUIList.Remove(item);
                                    numStatue--;
                                    break;
                                }
                            }
                        }
                        #endregion

                        movetoDest = false;
                    }
                }
            }
        }
    }

    #region ArrowMovement
    public void movestatueTL() {
        if (SelectionManager.selected && !movetoDest) {
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
        if (SelectionManager.selected && !movetoDest) {
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
        if (SelectionManager.selected && !movetoDest) {
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
        if (SelectionManager.selected && !movetoDest) {
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

    #endregion

    public void ToPlayerScene() {
        if (GameObject.Find("Highlight(Clone)") != null) {
            SelectionManager.selectedObject = null;
            SelectionManager.RemoveHighlight();
            SelectionManager.selected = false;
        }
        SceneManager.LoadScene(playerScene);
    }
    public static string GetPlayerScene() {
        return sceneSwitchTo;
    }
    public static List<Vector3> GetActivatorPositions() {
        return activatorPosition;
    }
    public static void ClearActivatorPositions() {
        activatorPosition.Clear();
    }
}
