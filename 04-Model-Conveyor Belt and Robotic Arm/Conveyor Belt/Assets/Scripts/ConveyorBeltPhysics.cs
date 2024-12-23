using UnityEngine;

public class ConveyorBeltPhysics : MonoBehaviour {
    public float speed = 0.1f; // Speed of the conveyor belt
    public StartButtonControl script; // Reference to the StartButton script
    public ConveyorSpeedControl SpeedControl;
    Rigidbody conv; // Rigidbody component of the Conveyor Belt
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        conv = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the Conveyor Belt

        Quaternion rot = transform.rotation;
        Debug.Log("Rotation: " + rot);
        Debug.Log("Rotation: " + rot.eulerAngles);
        Debug.Log("Rotation: " + rot.eulerAngles.y);
    }

    // FixedUpdate is called once per frame
    void FixedUpdate() {
        if(script.start){
            Vector3 pos = conv.position; // Get the position of the Conveyor Belt
            // To move the Rigidbody in the direction the GameObject is facing, you can use "transform.right" instead of "Vector3.right"
            conv.position = conv.position + transform.right * SpeedControl.ConveyorSpeed * Time.deltaTime; 
            conv.MovePosition(pos); // Update the position of the Conveyor Belt
        }
        
    }
}
