using UnityEngine;

public class Translate : MonoBehaviour {
    public float speed = 0.2f; // Speed of the translation
    public bool reverse = false; // Flag to indicate if the translation should be reversed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        float translation = speed * Time.deltaTime; // Calculate the translation based on the speed and elapsed time
        
        if (reverse) {
            translation = -translation; // Reverse the translation if the flag is set
        }

        // Translate the GameObject along the y-axis
        // This achieves the same result, transform.Translate(0, translation, 0); 
        transform.Translate(Vector3.up * translation); 
           
    }

    void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Sensor")) {
            // Debug.Log("Touch Sensor");
            reverse = true; // reverse the translation
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.CompareTag("Sensor")) {
            // Debug.Log("Exit Sensor");
        }
    }
}
