using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public void OnPointerDown(PointerEventData eventData) {
        if (!GetComponent<Button>().interactable) return;
        FindObjectOfType<AudioManager>().Play("ClickDown");
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (!GetComponent<Button>().interactable) return;
        FindObjectOfType<AudioManager>().Play("ClickUp");
    }
}
