using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler {

    #region Variables

    private GameObject temp = null;
    private Vector3 firstStatue;
    private Transform returnTo = null;
    private RectTransform rectTransform;
    private static int prevPos;

    public GameObject selectTemp = null;
    public GameObject iconSelect;

    #endregion

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update() {
        if (selectTemp != null) {
            selectTemp.transform.position = new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z);
        }
    }
    public void OnBeginDrag(PointerEventData eventData) {
        #region Create Placeholder

        temp = new GameObject();
        temp.transform.SetParent(transform.parent);
        LayoutElement iconSize = temp.AddComponent<LayoutElement>();
        iconSize.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        iconSize.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        iconSize.flexibleWidth = 0;
        iconSize.flexibleHeight = 0;

        #endregion

        #region Switch Icon Positions Start

        prevPos = transform.GetSiblingIndex();
        firstStatue = StatueData.statueUIList[transform.GetSiblingIndex()];

        #endregion

        temp.transform.SetSiblingIndex(transform.GetSiblingIndex());
        returnTo = transform.parent;
        transform.SetParent(transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta;

        int newSI = returnTo.childCount;
        for (int i = 0; i < returnTo.childCount; i++) {
            if (transform.position.y > returnTo.GetChild(i).position.y) {
                newSI = i;
                if (temp.transform.GetSiblingIndex() < newSI) {
                    newSI--;
                }
                break;
            }
        }
        temp.transform.SetSiblingIndex(newSI);
    }
    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(returnTo);
        if (SceneManager.GetActiveScene().name.Contains("Player")) {
            if (GMPlayer.stepVal == 0) {
                transform.SetSiblingIndex(temp.transform.GetSiblingIndex());
                if (temp.transform.GetSiblingIndex() != prevPos + 1) {
                    if (GameObject.Find("TileHighlight(Clone)")) {
                        foreach (var tile in GameObject.FindGameObjectsWithTag("TileHighlight")) {
                            Destroy(tile);
                        }
                    }
                }
            }
            else {
                transform.SetSiblingIndex(prevPos);
                Debug.Log("You Must Reset Level Before Moving Icons");
            }
        }
        else {
            transform.SetSiblingIndex(temp.transform.GetSiblingIndex());
        }
        #region Switch Icon Positions End

        Vector3 secondStatue = StatueData.statueUIList[transform.GetSiblingIndex()];
        if (secondStatue != firstStatue) {
            Vector3 tempStatue = firstStatue;

            int j = prevPos;
            if (transform.GetSiblingIndex() < prevPos) {
                for (int i = 0; i < prevPos - transform.GetSiblingIndex(); i++) {
                    StatueData.statueUIList[j] = StatueData.statueUIList[j - 1];
                    j--;
                }
                StatueData.statueUIList[transform.GetSiblingIndex()] = tempStatue;
            }
            else {
                for (int i = 0; i < transform.GetSiblingIndex() - prevPos; i++) {
                    StatueData.statueUIList[j] = StatueData.statueUIList[j + 1];
                    j++;
                }
                StatueData.statueUIList[transform.GetSiblingIndex()] = tempStatue;
            }
            
            //Vector3 tempStatue = secondStatue;
            //StatueData.statueUIList[transform.GetSiblingIndex()] = firstStatue;
            //StatueData.statueUIList[prevPos] = tempStatue;
            if (SceneManager.GetActiveScene().name.Contains("Player")) GMPlayer.highlightVal = -1;
        }
        #endregion
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(temp);
        temp = null;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Contains("Player")) {
            foreach (var item in GameObject.FindGameObjectsWithTag("StatueIcon")) {
                if (item != gameObject) {
                    if (item.GetComponent<ClickDrag>().selectTemp != null) {
                        item.GetComponent<ClickDrag>().DestroyIconSelection();
                    }
                }
            }
            if (selectTemp == null) {
                MakeIconSelection();
            }
            else {
                DestroyIconSelection();
            }
        }
        else if (SelectionManager.objecttoMove != null && SelectionManager.objecttoMove.transform.position != StatueData.statueUIList[transform.GetSiblingIndex()]) {
            SelectionManager.RemoveHighlight();
            Debug.Log("Change");
            foreach (var item in GameObject.FindGameObjectsWithTag("StatueIcon")) {
                if (item != gameObject) {
                    if (item.GetComponent<ClickDrag>().selectTemp != null) {
                        item.GetComponent<ClickDrag>().DestroyIconSelection();
                    }
                }
            }
            foreach (var statue in GameObject.FindGameObjectsWithTag("Statue")) {
                if (StatueData.statueUIList[transform.GetSiblingIndex()] == statue.transform.position) {
                    SelectionManager.objecttoMove = statue;
                    SelectionManager.selectedObject = statue.name;
                    SelectionManager.AddHighlight();
                    break;
                }
            }
            MakeIconSelection();
        }
        else if (SelectionManager.objecttoMove == null) {
            Debug.Log("Add");
            foreach (var statue in GameObject.FindGameObjectsWithTag("Statue")) {
                if (StatueData.statueUIList[transform.GetSiblingIndex()] == statue.transform.position) {
                    SelectionManager.objecttoMove = statue;
                    SelectionManager.AddHighlight();
                    SelectionManager.selected = true;
                    break;
                }
            }
            MakeIconSelection();
        }
        else {
            Debug.Log("Remove");
            SelectionManager.objecttoMove = null;
            SelectionManager.selectedObject = null;
            SelectionManager.selected = false;
            SelectionManager.RemoveHighlight();
            DestroyIconSelection();
        }
    }
    public void DestroyIconSelection() {
        if (GameObject.FindGameObjectsWithTag("SelectHighlight").Length != 0) {
            foreach (var highlight in GameObject.FindGameObjectsWithTag("SelectHighlight")) {
                Destroy(highlight);
            }
        }
        Destroy(selectTemp);
        selectTemp = null;
    }
    public void MakeIconSelection() {
        if (selectTemp == null) {
            selectTemp = Instantiate(iconSelect);
        }
    }
}
