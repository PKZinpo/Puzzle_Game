using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GMPlayer : MonoBehaviour {

    #region GeneralVariables

    public float movementSpeed = 1f;
    public string statueScene;
    public string nextlevelScene;
    public GameObject nextLevel;
    public GameObject playerObject;
    public GameObject tileHighlightPrefab;
    public GameObject tileselectHighlightPrefab;
    public GameObject shadowPrefab;
    public GameObject iceOverPrefab;
    public GameObject currentStep;
    public GameObject tutorialObject;
    public Tilemap currentMap;
    public bool nextlevelOnWall;

    public static Tilemap currentMapStatic;
    public static int highlightVal;
    public static int stepVal;
    public static GameObject tileHighlight;
    public static GameObject shadow;

    private int prevStepVal;
    private int tutorialVal;
    private bool inTutorial = false;
    private bool lvl1Dialogue = false;
    private bool playButton = false;
    private bool arrowButton = false;
    private bool statueList = false;
    private bool switchButton = false;
    private bool hideWall = false;
    private Vector3Int direction;
    private Vector3 collectableOffset = new Vector3(0.0f, -0.005f, 0.0f);
    private GameObject tile = null;
    private GameObject wallObject = null;

    private static string sceneSwitchTo;
    private static Vector3 nextlevelOffset;

    #endregion

    #region Level1Variables

    private static Vector3Int[] tilePositions = new Vector3Int[3];
    
    public static bool lvl1Path = false;
    private Vector3Int lvl1tilePlace;

    #endregion

    void Awake() {
        currentMapStatic = currentMap;
        sceneSwitchTo = statueScene;
        stepVal = 0;
        prevStepVal = stepVal;
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
        if (GameObject.FindGameObjectsWithTag("Collectable").Length != 0) {
            foreach (var collectable in GameObject.FindGameObjectsWithTag("Collectable")) {
                if (collectable.transform.position != currentMap.CellToWorld(currentMap.WorldToCell(collectable.transform.position))) {
                    collectable.transform.position = currentMap.CellToWorld(currentMap.WorldToCell(collectable.transform.position)) + collectableOffset;
                }
                if (!collectable.GetComponent<Collectable>().isOnWall) {
                    collectable.GetComponent<SortingGroup>().sortingLayerName = "Ground";
                }
                else {
                    collectable.GetComponent<SortingGroup>().sortingLayerName = "1st Floor";
                }
            }
        }
        if (StatueData.statueUIList.Count != 0) {
            Instantiate(currentStep, GameObject.FindGameObjectWithTag("CurrentStep").transform, false);
        }
    }

    void Start() {

        if (SceneManager.GetActiveScene().name == "Player1") {
            if (!inTutorial) {
                tutorialObject.SetActive(true);
                gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
        }
    }

    void Update() {

        #region Level1 Tutorial

        if (tutorialObject.activeSelf) {
            if (Input.anyKeyDown) {
                if (SceneManager.GetActiveScene().name == "Player1") {
                    tutorialVal++;
                    if (tutorialVal == 1) {
                        FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    }
                    else if (tutorialVal == 2) {
                        FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    }
                    else if (tutorialVal == 3) {
                        FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    }
                    else if (tutorialVal == 4) {
                        FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    }
                    else if (tutorialVal == 5) {
                        FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    }
                    else {

                    }
                    //if (!playButton) {
                    //    tutorialObject.transform.GetChild(0).gameObject.SetActive(true);
                    //    playButton = true;
                    //    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    //}
                    //else if (!arrowButton) {
                    //    tutorialObject.transform.GetChild(0).gameObject.SetActive(false);
                    //    tutorialObject.transform.GetChild(1).gameObject.SetActive(true);
                    //    arrowButton = true;
                    //    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    //}
                    //else if (!statueList) {
                    //    tutorialObject.transform.GetChild(1).gameObject.SetActive(false);
                    //    tutorialObject.transform.GetChild(2).gameObject.SetActive(true);
                    //    statueList = true;
                    //    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    //}
                    //else if (!switchButton) {
                    //    tutorialObject.transform.GetChild(2).gameObject.SetActive(false);
                    //    tutorialObject.transform.GetChild(3).gameObject.SetActive(true);
                    //    switchButton = true;
                    //    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    //}
                    //else {
                    //    tutorialObject.SetActive(false);
                    //}
                }
            }
        }

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

        if (GameObject.FindGameObjectsWithTag("StatueIcon").Length != 0) {
            foreach (var icon in GameObject.FindGameObjectsWithTag("StatueIcon")) {
                if (icon.GetComponent<ClickDrag>().selectTemp != null) {
                    if (GameObject.FindGameObjectsWithTag("SelectHighlight").Length == 0) {
                        AddSelectTileHighlight(icon.transform.GetSiblingIndex());
                        break;
                    }
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
            if (nextlevelOnWall) {
                if (nextLevel.activeSelf) {
                    GameObject.FindGameObjectWithTag("NextLevel").GetComponentInChildren<Animator>().SetTrigger("HideOn");
                }
            }
            if (GameObject.FindGameObjectsWithTag("TileHighlight").Length != 0) {
                foreach (var highlight in GameObject.FindGameObjectsWithTag("TileHighlight")) {
                    if (highlight.GetComponent<SpriteRenderer>().sortingLayerName == "1st Floor") {
                        Color color = highlight.GetComponent<SpriteRenderer>().color;
                        if (color.a >= 0) {
                            color.a -= 0.01f;
                            highlight.GetComponent<SpriteRenderer>().color = color;
                        }
                    }
                }
            }
            if (GameObject.FindGameObjectsWithTag("SelectHighlight").Length != 0) {
                foreach (var highlight in GameObject.FindGameObjectsWithTag("SelectHighlight")) {
                    if (highlight.GetComponent<SpriteRenderer>().sortingLayerName == "1st Floor") {
                        Color color = highlight.GetComponent<SpriteRenderer>().color;
                        if (color.a >= 0) {
                            color.a -= 0.01f;
                            highlight.GetComponent<SpriteRenderer>().color = color;
                        }
                    }
                }
            }
            if (GameObject.FindGameObjectsWithTag("Collectable").Length != 0) {
                foreach (var collectable in GameObject.FindGameObjectsWithTag("Collectable")) {
                    if (collectable.GetComponent<Collectable>().isOnWall) {
                        Color color = collectable.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                        Color shadowColor = collectable.transform.GetChild(1).GetComponent<SpriteRenderer>().color;
                        if (color.a >= 0) {
                            color.a -= 0.01f;
                            shadowColor.a -= 0.01f;
                            collectable.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                            collectable.transform.GetChild(1).GetComponent<SpriteRenderer>().color = shadowColor;
                        }
                    }
                }
            }
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
            if (nextlevelOnWall) {
                if (nextLevel.activeSelf) {
                    GameObject.FindGameObjectWithTag("NextLevel").GetComponentInChildren<Animator>().SetTrigger("HideOff");
                }
            }
            if (GameObject.FindGameObjectsWithTag("TileHighlight").Length != 0) {
                foreach (var highlight in GameObject.FindGameObjectsWithTag("TileHighlight")) {
                    if (highlight.GetComponent<SpriteRenderer>().sortingLayerName == "1st Floor") {
                        Color color = highlight.GetComponent<SpriteRenderer>().color;
                        if (color.a <= 0.59f) {
                            color.a += 0.01f;
                            highlight.GetComponent<SpriteRenderer>().color = color;
                        }
                    }
                }
            }
            if (GameObject.FindGameObjectsWithTag("SelectHighlight").Length != 0) {
                foreach (var highlight in GameObject.FindGameObjectsWithTag("SelectHighlight")) {
                    if (highlight.GetComponent<SpriteRenderer>().sortingLayerName == "1st Floor") {
                        Color color = highlight.GetComponent<SpriteRenderer>().color;
                        if (color.a <= 0.59f) {
                            color.a += 0.01f;
                            highlight.GetComponent<SpriteRenderer>().color = color;
                        }
                    }
                }
            }
            if (GameObject.FindGameObjectsWithTag("Collectable").Length != 0) {
                foreach (var collectable in GameObject.FindGameObjectsWithTag("Collectable")) {
                    if (collectable.GetComponent<Collectable>().isOnWall) {
                        Color color = collectable.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                        Color shadowColor = collectable.transform.GetChild(1).GetComponent<SpriteRenderer>().color;
                        if (color.a <= 1) {
                            color.a += 0.01f;
                            shadowColor.a += 0.01f;
                            collectable.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                            collectable.transform.GetChild(1).GetComponent<SpriteRenderer>().color = shadowColor;
                        }
                    }
                }
            }
        }

        if (GameObject.FindGameObjectsWithTag("Collectable").Length != 0) {
            if (nextLevel.activeSelf) {
                nextLevel.SetActive(false);
            }
            foreach (var collectable in GameObject.FindGameObjectsWithTag("Collectable")) {
                if (currentMap.WorldToCell(playerObject.transform.position) == currentMap.WorldToCell(collectable.transform.position - collectableOffset)) {
                    Destroy(collectable);
                }
            }
        }
        else {
            if (!nextLevel.activeSelf) {
                nextLevel.SetActive(true);
            }
            if (!PlayerMovement.movetoDest) {
                if (GameObject.FindGameObjectWithTag("NextLevel")) {
                    if (currentMap.WorldToCell(playerObject.transform.position) == currentMap.WorldToCell(nextLevel.transform.GetChild(0).transform.position)) {
                        ToNextLevel();
                    }
                }
            }
        }

        if (prevStepVal != stepVal) {
            foreach (var step in GameObject.FindGameObjectsWithTag("NextStep")) {
                Destroy(step);
            }
            if (stepVal != StatueData.statueUIList.Count) {
                for (int i = 0; i < stepVal + 1; i++) {
                    GameObject step = Instantiate(currentStep, GameObject.FindGameObjectWithTag("CurrentStep").transform, false);
                    if (i != stepVal) {
                        Color stepColor = step.GetComponent<Image>().color;
                        stepColor.a = 0;
                        step.GetComponent<Image>().color = stepColor;
                    }
                    else {
                        prevStepVal = stepVal;
                    }
                }
            }
            else {
                prevStepVal = stepVal;
            }
        }
    }

    public static string GetStatueScene() {
        return sceneSwitchTo;
    }
    public void NextStep() {
        if (!TileMoving.isMoving) {
            if (GameObject.FindGameObjectsWithTag("SelectHighlight").Length != 0) {
                foreach (var tile in GameObject.FindGameObjectsWithTag("SelectHighlight")) {
                    Destroy(tile);
                }
            }
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
                        ChooseType(item.Value.type, stepVal);
                        foreach (var icon in GameObject.FindGameObjectsWithTag("StatueIcon")) {
                            if (icon.GetComponent<ClickDrag>().selectTemp != null) {
                                Destroy(icon.GetComponent<ClickDrag>().selectTemp);
                            }
                            if (icon.transform.GetSiblingIndex() == stepVal) {
                                icon.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                icon.GetComponent<CanvasGroup>().interactable = false;
                                icon.GetComponent<CanvasGroup>().alpha = 0.3f;
                            }
                        }
                        stepVal++;
                        break;
                    }
                }
            }
        }
    }
    public void ChooseType(string type, int stepNum) {
        switch (type) {
            case "3Line":
                TileMoving.MoveTiles(ThreeLine(stepNum));
                break;

            case "Ice":
                IceOver(IceOverPositions(stepNum));
                break;
        }
    }
    public Vector3Int[] ThreeLine(int num) {
        Vector3Int[] tilePlaceArray = new Vector3Int[3];
        var statue = StatueData.statueUIList[num];
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
    private Vector3Int[] IceOverPositions(int num) {
        var statuePos = currentMap.WorldToCell(StatueData.statueUIList[num]);
        List<Vector3Int> tempPos = new List<Vector3Int>();
        var tileAmount = 0;

        if (currentMap.HasTile(statuePos) || WallCheck(statuePos)) {
            tempPos.Add(statuePos);
            tileAmount++;
        }
        if (currentMap.HasTile(statuePos + Vector3Int.left) || WallCheck(statuePos + Vector3Int.left)) {
            tempPos.Add(statuePos + Vector3Int.left);
            tileAmount++;
        }
        if (currentMap.HasTile(statuePos + Vector3Int.up) || WallCheck(statuePos + Vector3Int.up)) {
            tempPos.Add(statuePos + Vector3Int.up);
            tileAmount++;
        }
        if (currentMap.HasTile(statuePos + Vector3Int.down) || WallCheck(statuePos + Vector3Int.down)) {
            tempPos.Add(statuePos + Vector3Int.down);
            tileAmount++;
        }
        if (currentMap.HasTile(statuePos + Vector3Int.right) || WallCheck(statuePos + Vector3Int.right)) {
            tempPos.Add(statuePos + Vector3Int.right);
            tileAmount++;
        }

        Vector3Int[] iceTileArray = new Vector3Int[tileAmount];
        for (int i = 0; i < tileAmount; i++) {
            iceTileArray[i] = tempPos[i];
        }


        return iceTileArray;
    }
    private void IceOver(Vector3Int[] positions) {
        for (int i = 0; i < positions.Length; i++) {
            string tileType;
            bool isWall = false;
            foreach (var wall in GameObject.FindGameObjectsWithTag("Wall")) {
                if (wall.transform.position == currentMap.CellToWorld(positions[i]) + TileMoving.wallOffset) {
                    isWall = true;
                    wallObject = wall;
                }
            }
            if (isWall) {
                tileType = wallObject.GetComponentInChildren<SpriteRenderer>().sprite.name;
                if (tileType.Contains("Exit Tile")) {
                    Debug.Log("Cannot freeze Tile");
                }
                else {
                    tile = Instantiate(iceOverPrefab);
                    switch (tileType) {
                        case "Ground Half Tile":
                            tile.GetComponent<Animator>().SetTrigger("GroundHalfWall");
                            break;

                        case "Broken Tile":
                            tile.GetComponent<Animator>().SetTrigger("BrokenWall");
                            break;
                    }
                    tile.transform.SetParent(wallObject.transform);
                    tile.transform.localPosition = new Vector3(0.0f, 0.16f, 0.0f) - TileMoving.wallOffset;
                }
            }
            else {
                tileType = currentMap.GetTile(positions[i]).name;
                if (tileType.Contains("Exit Tile")) {
                    Debug.Log("Cannot freeze Tile");
                }
                else {
                    tile = Instantiate(iceOverPrefab);
                    switch (tileType) {
                        case "Tilemap Ground Half":
                            tile.GetComponent<Animator>().SetTrigger("GroundHalfFloor");
                            break;

                        case "Tilemap Broken":
                            tile.GetComponent<Animator>().SetTrigger("BrokenFloor");
                            break;
                    }
                    tile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                    tile.transform.position = currentMap.CellToWorld(positions[i]);
                }
            }
        }
    }
    private void AddTileHighlight() {
        foreach (var item in StatueData.statueList) {
            if (StatueData.statueUIList[stepVal] == item.Key) {
                switch (item.Value.type) {
                    case "3Line":
                        for (int i = 0; i < ThreeLine(stepVal).Length; i++) {
                            tileHighlight = Instantiate(tileHighlightPrefab);
                            var wallCheck = new Vector3Int(ThreeLine(stepVal)[i].x + 1, ThreeLine(stepVal)[i].y + 1, 0);
                            bool isWall = false;
                            foreach (var wall in GameObject.FindGameObjectsWithTag("Wall")) {
                                if (wall.transform.position == currentMap.CellToWorld(ThreeLine(stepVal)[i]) + TileMoving.wallOffset) {
                                    isWall = true;
                                }
                            }
                            if (isWall) {
                                tileHighlight.transform.position = currentMap.CellToWorld(wallCheck);
                                tileHighlight.GetComponent<SpriteRenderer>().sortingLayerName = "1st Floor";
                            }
                            else {
                                tileHighlight.transform.position = currentMap.CellToWorld(ThreeLine(stepVal)[i]);
                            }
                        }
                        break;

                    case "Ice":
                        for (int i = 0; i < IceOverPositions(stepVal).Length; i++) {
                            tileHighlight = Instantiate(tileHighlightPrefab);
                            var wallCheck = new Vector3Int(IceOverPositions(stepVal)[i].x + 1, IceOverPositions(stepVal)[i].y + 1, 0);
                            bool isWall = false;
                            foreach (var wall in GameObject.FindGameObjectsWithTag("Wall")) {
                                if (wall.transform.position == currentMap.CellToWorld(IceOverPositions(stepVal)[i]) + TileMoving.wallOffset) {
                                    isWall = true;
                                    break;
                                }
                            }
                            if (isWall) {
                                tileHighlight.transform.position = currentMap.CellToWorld(wallCheck);
                                tileHighlight.GetComponent<SpriteRenderer>().sortingLayerName = "1st Floor";
                            }
                            else {
                                tileHighlight.transform.position = currentMap.CellToWorld(IceOverPositions(stepVal)[i]);
                            }
                        }
                        break;
                }
            }
        }
    }
    public void AddSelectTileHighlight(int siblingIndex) {
        foreach (var item in StatueData.statueList) {
            if (StatueData.statueUIList[siblingIndex] == item.Key) {
                switch (item.Value.type) {
                    case "3Line":
                        for (int i = 0; i < ThreeLine(siblingIndex).Length; i++) {
                            tileHighlight = Instantiate(tileselectHighlightPrefab);
                            var wallCheck = new Vector3Int(ThreeLine(siblingIndex)[i].x + 1, ThreeLine(siblingIndex)[i].y + 1, 0);
                            bool isWall = false;
                            foreach (var wall in GameObject.FindGameObjectsWithTag("Wall")) {
                                if (wall.transform.position == currentMap.CellToWorld(ThreeLine(siblingIndex)[i]) + TileMoving.wallOffset) {
                                    isWall = true;
                                }
                            }
                            if (isWall) {
                                tileHighlight.transform.position = currentMap.CellToWorld(wallCheck);
                                tileHighlight.GetComponent<SpriteRenderer>().sortingLayerName = "1st Floor";
                            }
                            else {
                                tileHighlight.transform.position = currentMap.CellToWorld(ThreeLine(siblingIndex)[i]);
                            }
                        }
                        break;

                    case "Ice":
                        for (int i = 0; i < IceOverPositions(siblingIndex).Length; i++) {
                            tileHighlight = Instantiate(tileselectHighlightPrefab);
                            var wallCheck = new Vector3Int(IceOverPositions(siblingIndex)[i].x + 1, IceOverPositions(siblingIndex)[i].y + 1, 0);
                            bool isWall = false;
                            foreach (var wall in GameObject.FindGameObjectsWithTag("Wall")) {
                                if (wall.transform.position == currentMap.CellToWorld(IceOverPositions(siblingIndex)[i]) + TileMoving.wallOffset) {
                                    isWall = true;
                                    break;
                                }
                            }
                            if (isWall) {
                                tileHighlight.transform.position = currentMap.CellToWorld(wallCheck);
                                tileHighlight.GetComponent<SpriteRenderer>().sortingLayerName = "1st Floor";
                            }
                            else {
                                tileHighlight.transform.position = currentMap.CellToWorld(IceOverPositions(siblingIndex)[i]);
                            }
                        }
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
    private bool WallCheck(Vector3Int position) {
        foreach (var item in GameObject.FindGameObjectsWithTag("Wall")) {
            if (item.transform.position == currentMap.CellToWorld(position) + TileMoving.wallOffset) {
                return true;
            }
        }
        return false;
    }
}
