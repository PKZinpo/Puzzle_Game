using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GMStatue : MonoBehaviour {

    #region GeneralVariables

    public GameObject tutorialObject;
    public Tilemap currentMap;
    public float movementSpeed = 1f;
    public string playerScene;

    public static int numStatue;

    private int tutorialVal = 0;
    private Vector3Int togridPos;
    private Vector3Int selectedgridPos;
    private AudioSource statueMove;

    private static GameObject moveObject;
    private static bool movetoDest = false;
    private static Vector3 destination;
    private static string sceneSwitchTo;
    private static List<Vector3> activatorPosition = new List<Vector3>();

    #endregion

    #region Level1Variables


    #endregion

    void Awake() {

        sceneSwitchTo = playerScene;

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
            foreach (var item in StatueData.statueList) {
                foreach (var statue in GameObject.FindGameObjectsWithTag("Statue")) {
                    if (statue.GetComponent<StatueType>().statueType == item.Value.type && !statue.GetComponent<StatueType>().isPlaced) {
                        statue.transform.position = item.Key;
                        statue.GetComponent<StatueType>().yAxis = item.Value.isTurned;
                        statue.GetComponent<StatueType>().isOn = item.Value.isActive;
                        statue.GetComponent<StatueType>().isPlaced = true;
                        break;
                    }
                }
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

        List<Vector3> tempStatue = new List<Vector3>();

        foreach (var item in GameObject.FindGameObjectsWithTag("Statue")) {
            foreach (var position in activatorPosition) {
                if (item.transform.position == position) {
                    tempStatue.Add(position);
                    break;
                }
            }
            if (activatorPosition.Contains(item.transform.position)) {
                if (numStatue < tempStatue.Count) {
                    foreach (var activator in GameObject.FindGameObjectsWithTag("Activator")) {
                        if (activator.transform.position == item.transform.position) {
                            activator.GetComponent<Animator>().SetTrigger("On");
                            break;
                        }
                    }
                    StatueData.statueUIList.Add(item.transform.position);
                    numStatue++;
                    item.GetComponent<StatueType>().onSwitch = true;
                }
            }
        }
    }

    void Start() {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Statue1") {
            if (!FindObjectOfType<GameObjectData>().lvl1StatueTutorial) {
                tutorialObject.SetActive(true);
                gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
        }
        else if (sceneName == "Statue2") {
            if (!FindObjectOfType<GameObjectData>().lvl2StatueTutorial) {
                tutorialObject.SetActive(true);
                gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
        }

        for (int i = 0; i < FindObjectOfType<AudioManager>().GetComponents<AudioSource>().Length; i++) {
            if (FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i].clip.name == "Statue Move (TEST)") {
                statueMove = FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i];
            }
        }
    }

    void Update() {

        #region Tutorial

        if (tutorialObject != null && tutorialObject.activeSelf) {
            bool typingCheck = FindObjectOfType<DialogueManager>().isTyping;
            if (Input.anyKeyDown && !typingCheck) {
                string sceneName = SceneManager.GetActiveScene().name;
                tutorialVal++;
                if (sceneName == "Statue1") {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                }
                else if (sceneName == "Statue2") {
                    tutorialObject.transform.GetChild(4).gameObject.SetActive(true);
                    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                }
                if (gameObject.GetComponent<DialogueTrigger>().dialogue.sentences.Length == tutorialVal) {
                    switch (sceneName) {
                        case "Statue1":
                            FindObjectOfType<GameObjectData>().lvl1StatueTutorial = true;
                            Debug.Log("LVL1StatueDONE");
                            break;

                        case "Statue2":
                            FindObjectOfType<GameObjectData>().lvl2StatueTutorial = true;
                            Debug.Log("LVL2StatueDONE");
                            break;
                    }
                }
            }
        }

        #endregion

        if (SelectionManager.selected) {
            moveObject = SelectionManager.objecttoMove;
            if (GameObject.FindGameObjectsWithTag("TurnButton").Length != 0) {
                if (moveObject.GetComponent<StatueType>().statueType != "3Line") {
                    GameObject.FindGameObjectWithTag("TurnButton").GetComponent<Button>().interactable = false;
                }
                else {
                    GameObject.FindGameObjectWithTag("TurnButton").GetComponent<Button>().interactable = true;
                }
            }
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
                                    break;
                                }
                            }
                        }
                        if (activatorPosition.Contains(moveObject.transform.position)) {
                            if (numStatue < tempStatue.Count) {
                                ActivatorSwitchOn();
                                StatueData.statueUIList.Add(moveObject.transform.position);
                                numStatue++;
                                Debug.Log("Added position " + currentMap.WorldToCell(moveObject.transform.position));
                                moveObject.GetComponent<StatueType>().onSwitch = true;
                            }
                            else {
                                for (int i = 0; i < tempStatue.Count; i++) {
                                    if (!tempStatue.Contains(StatueData.statueUIList[i])) {
                                        for (int j = 0; j < tempStatue.Count; j++) {
                                            if (!StatueData.statueUIList.Contains(tempStatue[j])) {
                                                Debug.Log("Switched position " + currentMap.WorldToCell(StatueData.statueUIList[i]) + " to " + currentMap.WorldToCell(tempStatue[j]));
                                                ActivatorSwitchOff(StatueData.statueUIList[i]);
                                                StatueData.statueUIList[i] = tempStatue[j];
                                                ActivatorSwitchOn();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            foreach (var item in StatueData.statueUIList) {
                                if (!tempStatue.Contains(item)) {
                                    ActivatorSwitchOff(item);
                                    Debug.Log("Removed position " + currentMap.WorldToCell(item));
                                    StatueData.statueUIList.Remove(item);
                                    moveObject.GetComponent<StatueType>().onSwitch = true;
                                    numStatue--;
                                    FindObjectOfType<AudioManager>().Play("StatueOff");
                                    break;
                                }
                            }
                        }
                        #endregion

                        movetoDest = false;
                        EnableButtons();
                    }
                }
            }
        }
        else {
            if (GameObject.FindGameObjectsWithTag("TurnButton").Length != 0) {
                GameObject.FindGameObjectWithTag("TurnButton").GetComponent<Button>().interactable = false;
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
                FindObjectOfType<AudioManager>().Play("StatueMove");
                statueMove.volume = 0.5f;
                statueMove.pitch = 0.9f;
                DisableButtons();
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
                FindObjectOfType<AudioManager>().Play("StatueMove");
                statueMove.volume = 0.5f;
                statueMove.pitch = 0.9f;
                DisableButtons();
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
                FindObjectOfType<AudioManager>().Play("StatueMove");
                statueMove.volume = 0.5f;
                statueMove.pitch = 0.9f;
                DisableButtons();
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
                FindObjectOfType<AudioManager>().Play("StatueMove");
                statueMove.volume = 0.5f;
                statueMove.pitch = 0.9f;
                DisableButtons();
            }
        }
    }
    #endregion

    #region Level1

    #endregion
    public void DisableButtons() {
        GameObject arrows = GameObject.Find("Canvas").transform.Find("Arrows").gameObject;
        for (int i = 0; i < arrows.transform.childCount; i++) {
            arrows.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }
    public void EnableButtons() {
        GameObject arrows = GameObject.Find("Canvas").transform.Find("Arrows").gameObject;
        for (int i = 0; i < arrows.transform.childCount; i++) {
            arrows.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }
    public static string GetPlayerScene() {
        return sceneSwitchTo;
    }
    public static void ClearActivatorPositions() {
        activatorPosition.Clear();
    }
    public void ActivatorSwitchOn() {
        foreach (var activator in GameObject.FindGameObjectsWithTag("Activator")) {
            if (activator.transform.position == moveObject.transform.position) {
                activator.GetComponent<Animator>().SetTrigger("On");
                FindObjectOfType<AudioManager>().Play("StatueOn");
                break;
            }
        }
    }
    public void ActivatorSwitchOff(Vector3 position) {
        foreach (var activator in GameObject.FindGameObjectsWithTag("Activator")) {
            if (activator.transform.position == position) {
                activator.GetComponent<Animator>().SetTrigger("Off");
                break;
            }
        }
    }
}
