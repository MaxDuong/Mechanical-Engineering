using UnityEngine;

public class SensorDetection : MonoBehaviour {
    public bool isDetected = false; // Boolean to check if the sensor is detecting an object
    public int count = 0; // Counter to keep track of the number of objects detected
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {}

    // Update is called once per frame
    void Update() {}

    // This method is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other) {
        isDetected = true; // Set the isDetected boolean to true
        count++;
    }
    
    private void OnTriggerExit(Collider other) {
        isDetected = false;
        // if (isDetected){
        //     isDetected = false; // Set the isDetected boolean to false
        //     count++; // Increment the count
        // }
        
    }
}
