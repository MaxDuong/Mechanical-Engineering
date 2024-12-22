using UnityEngine;

public class ConveyorSpeedControl : MonoBehaviour {
    public float ConveyorPulleysSpeed = 50.0f;
    public float BeltSpeed;
    public float ConveyorSpeed; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        BeltSpeed = ConveyorPulleysSpeed / 100 * -1;
        ConveyorSpeed = ConveyorPulleysSpeed / 100 * 1.2f;
    }

    // Update is called once per frame
    void Update() {
        BeltSpeed = ConveyorPulleysSpeed / 100 * -1;
        ConveyorSpeed = ConveyorPulleysSpeed / 100 * 1.2f;
    }
}
