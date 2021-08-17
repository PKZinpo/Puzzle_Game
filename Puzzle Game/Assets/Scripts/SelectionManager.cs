using UnityEngine;

public class SelectionManager : MonoBehaviour {

    #region Variables

    public GameObject highlightPrefab;

    private static GameObject highlight;

    public static GameObject hlPrefab;
    public static GameObject objecttoMove;
    public static string selectedObject;
    public static bool selected = false;

    #endregion
    void Awake() {
        hlPrefab = highlightPrefab;
    }
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit) {
                if (selected == true && hit.collider.gameObject.name != selectedObject) {
                    RemoveHighlight();
                    selectedObject = hit.collider.gameObject.name;
                    objecttoMove = GameObject.Find(selectedObject);
                    AddHighlight();
                }
                else if (selected) {
                    selectedObject = null;
                    objecttoMove = null;
                    RemoveHighlight();
                    selected = false;
                }
                else {
                    selectedObject = hit.collider.gameObject.name;
                    objecttoMove = GameObject.Find(selectedObject);
                    AddHighlight();
                    selected = true;
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
        }
    }
}
