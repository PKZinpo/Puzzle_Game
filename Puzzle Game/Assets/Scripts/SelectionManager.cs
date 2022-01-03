using UnityEngine;

public class SelectionManager : MonoBehaviour {

    #region Variables

    public GameObject highlightPrefab;

    

    private static GameObject highlight;
    private static AudioSource statueMove;

    public static GameObject hlPrefab;
    public static GameObject objecttoMove;
    public static string selectedObject;
    public static bool selected = false;

    #endregion
    void Awake() {
        hlPrefab = highlightPrefab;

        for (int i = 0; i < FindObjectOfType<AudioManager>().GetComponents<AudioSource>().Length; i++) {
            if (FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i].clip.name == "Statue Move (TEST)") {
                statueMove = FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[i];
            }
        }
    }
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit) {
                if (selected == true && hit.collider.gameObject.name != selectedObject) {
                    if (GameObject.FindGameObjectsWithTag("StatueIcon").Length != 0) {
                        GameObject iconParent = GameObject.FindGameObjectWithTag("StatueIcon").transform.parent.gameObject;
                        for (int i = 0; i < StatueData.statueUIList.Count; i++) {
                            if (iconParent.transform.GetChild(i).GetComponent<ClickDrag>().selectTemp != null) {
                                iconParent.transform.GetChild(i).GetComponent<ClickDrag>().DestroyIconSelection();
                                break;
                            }
                        }
                    }
                    RemoveHighlight();
                    selectedObject = hit.collider.gameObject.name;
                    objecttoMove = GameObject.Find(selectedObject);
                    AddHighlight();
                    if (GameObject.FindGameObjectsWithTag("StatueIcon").Length != 0) {
                        GameObject iconParent = GameObject.FindGameObjectWithTag("StatueIcon").transform.parent.gameObject;
                        for (int i = 0; i < StatueData.statueUIList.Count; i++) {
                            if (objecttoMove.transform.position == StatueData.statueUIList[i]) {
                                iconParent.transform.GetChild(i).GetComponent<ClickDrag>().MakeIconSelection();
                                break;
                            }
                        }
                    }
                }
                else if (selected) {
                    if (GameObject.FindGameObjectsWithTag("StatueIcon").Length != 0) {
                        GameObject iconParent = GameObject.FindGameObjectWithTag("StatueIcon").transform.parent.gameObject;
                        for (int i = 0; i < StatueData.statueUIList.Count; i++) {
                            if (objecttoMove.transform.position == StatueData.statueUIList[i]) {
                                iconParent.transform.GetChild(i).GetComponent<ClickDrag>().DestroyIconSelection();
                                break;
                            }
                        }
                    }
                    selectedObject = null;
                    objecttoMove = null;
                    RemoveHighlight();
                    selected = false;
                }
                else {
                    selectedObject = hit.collider.gameObject.name;
                    objecttoMove = GameObject.Find(selectedObject);
                    selected = true;
                    AddHighlight();
                    if (GameObject.FindGameObjectsWithTag("StatueIcon").Length != 0) {
                        GameObject iconParent = GameObject.FindGameObjectWithTag("StatueIcon").transform.parent.gameObject;
                        for (int i = 0; i < StatueData.statueUIList.Count; i++) {
                            if (objecttoMove.transform.position == StatueData.statueUIList[i]) {
                                iconParent.transform.GetChild(i).GetComponent<ClickDrag>().MakeIconSelection();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    public static void AddHighlight() {
        highlight = Instantiate(hlPrefab);
    }
    public static void RemoveHighlight() {
        Destroy(highlight.gameObject);
    }
    public static void TurnStatue() {
        if (selected) {
            objecttoMove.GetComponent<StatueType>().ToTurn();
            FindObjectOfType<AudioManager>().Play("StatueMove");
            statueMove.volume = 0.5f;
            statueMove.pitch = 1f;
        }
    }
    private bool CheckStatueList() {
        foreach (var pos in StatueData.statueUIList) {
            if (pos == objecttoMove.transform.position) {
                return true;
            }
        }
        return false;
    }
}
