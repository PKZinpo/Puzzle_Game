using UnityEngine;

public class SineMovement : MonoBehaviour {

    [SerializeField] private float frequency;
    [SerializeField] private float magnitude;
    [SerializeField] private float offset;
    private Vector3 startPosition;

    private void Start() {
        if (name.Contains("Selected")) {
            startPosition = SelectionManager.objecttoMove.transform.position;
        }
        else {
            startPosition = transform.parent.transform.position;
        }
    }

    void Update() {
        if (name.Contains("Selected")) {
            startPosition = SelectionManager.objecttoMove.transform.position;
        }
        transform.position = startPosition + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
    }
}
