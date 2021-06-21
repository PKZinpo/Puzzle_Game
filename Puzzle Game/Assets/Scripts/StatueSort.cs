using UnityEngine;
using UnityEngine.EventSystems;

public class StatueSort : MonoBehaviour, IDropHandler {

    #region Variables

    public GameObject statueIconPrefab;
    public GameObject iconParent;
    private GameObject statueIcon;
    private int listNum;

    #endregion

    void Awake() {
    }
    
    void Start() {
        foreach (var UIItem in StatueData.statueUIList) {
            foreach (var item in StatueData.statueList) {
                if (UIItem == item.Key) {
                    statueIcon = Instantiate(statueIconPrefab, iconParent.transform, false);
                    string typeName = item.Value.type;
                    foreach (var iconItem in StatueData.iconSprites) {
                        if (typeName == iconItem.name) {
                            statueIcon.GetComponent<UnityEngine.UI.Image>().sprite = iconItem;
                        }
                    }
                }
            }
        }
        listNum = StatueData.statueUIList.Count;

    }

    void Update() {
        if (listNum != StatueData.statueUIList.Count) {
            if (GameObject.FindGameObjectsWithTag("StatueIcon").Length != 0) {
                foreach (var icon in GameObject.FindGameObjectsWithTag("StatueIcon")) {
                    Destroy(icon);
                }
            }
            foreach (var UIItem in StatueData.statueUIList) {
                foreach (var item in StatueData.statueList) {
                    if (UIItem == item.Key) {
                        statueIcon = Instantiate(statueIconPrefab, iconParent.transform, false);
                        string typeName = item.Value.type;
                        foreach (var iconItem in StatueData.iconSprites) {
                            if (typeName == iconItem.name) {
                                statueIcon.GetComponent<UnityEngine.UI.Image>().sprite = iconItem;
                            }
                        }
                    }
                }
            }
            listNum = StatueData.statueUIList.Count;
        }
    }





    public void OnDrop(PointerEventData eventData) {
        //Debug.Log("Dropped");
    }

}
