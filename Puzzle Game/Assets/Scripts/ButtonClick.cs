using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public void OnPointerDown(PointerEventData eventData) {
        FindObjectOfType<AudioManager>().Play("ClickDown");
    }

    public void OnPointerUp(PointerEventData eventData) {
        FindObjectOfType<AudioManager>().Play("ClickUp");
    }
}
