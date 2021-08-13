using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ClickDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler {

    #region Variables

    private GameObject temp = null;
    private GameObject selectTemp = null;
    private Vector3 firstStatue;
    private Transform returnTo = null;
    private RectTransform rectTransform;
    private static int prevPos;

    public GameObject iconSelect;

    #endregion

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update() {
        if (SelectionManager.objecttoMove != null) {
            if (StatueData.statueUIList[transform.GetSiblingIndex()] == SelectionManager.objecttoMove.transform.position) {
                MakeIconSelection();
            }
            else {
                if (selectTemp != null) {
                    DestroyIconSelection();
                }
            }
        }
        else {
            if (selectTemp != null) {
                DestroyIconSelection();
            }
        }
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

        Debug.Log("Dragged");
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
        if (GMPlayer.stepVal == 0) {
            transform.SetSiblingIndex(temp.transform.GetSiblingIndex());
            if (GameObject.Find("TileHighlight(Clone)")) {
                foreach (var tile in GameObject.FindGameObjectsWithTag("TileHighlight")) {
                    Destroy(tile);
                }
            }
        }
        else {
            transform.SetSiblingIndex(prevPos);
            Debug.Log("You Must Reset Level Before Moving Icons");
        }

        #region Switch Icon Positions End

        Vector3 secondStatue = StatueData.statueUIList[transform.GetSiblingIndex()];
        if (secondStatue != firstStatue) {
            Vector3 tempStatue = secondStatue;
            StatueData.statueUIList[transform.GetSiblingIndex()] = firstStatue;
            StatueData.statueUIList[prevPos] = tempStatue;
            GMPlayer.highlightVal = -1;
        }

        #endregion

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(temp);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (SceneManager.GetActiveScene().name.Contains("Statue")) {

        }
        Debug.Log("Clicked");
    }
    public void DestroyIconSelection() {
        Destroy(selectTemp);
        selectTemp = null;
    }
    public void MakeIconSelection() {
        if (selectTemp == null) {
            selectTemp = Instantiate(iconSelect);
        }
    }
}
