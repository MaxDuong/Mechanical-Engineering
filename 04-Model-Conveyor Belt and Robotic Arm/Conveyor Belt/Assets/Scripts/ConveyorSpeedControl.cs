using UnityEngine;

public class ConveyorSpeedControl : MonoBehaviour {
    public float ConveyorPulleysSpeed = 50.0f;
    public float BeltSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        BeltSpeed = ConveyorPulleysSpeed / 100 * -1;
    }

    // Update is called once per frame
    void Update() {
        BeltSpeed = ConveyorPulleysSpeed / 100 * -1;
    }
}
