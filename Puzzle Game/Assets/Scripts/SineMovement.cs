using UnityEngine;

public class SineMovement : MonoBehaviour {

    [SerializeField] private float frequency;
    [SerializeField] private float magnitude;
    [SerializeField] private float offset;
    private Vector3 startPosition;

    void Update() {
        startPosition = SelectionManager.objecttoMove.transform.position;
        transform.position = startPosition + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
    }
}
