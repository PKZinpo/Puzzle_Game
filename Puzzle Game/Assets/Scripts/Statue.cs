using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Statue", menuName = "Statue")]
public class Statue : ScriptableObject {
    // Start is called before the first frame update
    public bool turn;
    public bool yAxis;

    public Sprite statueType;
}
