using UnityEngine;

public class ConveyorBeltPhysics : MonoBehaviour {
    public float speed = 0.1f; // Speed of the conveyor belt
    public StartButtonControl script; // Reference to the StartButton script
    public ConveyorSpeedControl SpeedControl;
    Rigidbody conv; // Rigidbody component of the Conveyor Belt
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        conv = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the Conveyor Belt
    }

    // FixedUpdate is called once per frame
    void FixedUpdate() {
        if(script.start){
            Vector3 pos = conv.position; // Get the position of the Conveyor Belt
            conv.position = conv.position + Vector3.right * SpeedControl.ConveyorSpeed * Time.deltaTime; // Move the Conveyor Belt to the left
            conv.MovePosition(pos); // Update the position of the Conveyor Belt
        }
        
    }
}
