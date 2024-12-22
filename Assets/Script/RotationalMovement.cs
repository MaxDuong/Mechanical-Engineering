using UnityEngine;

public class RotationalMovement : MonoBehaviour
{
    // Public variable to set degrees per second in the Unity editor
    // 1 Rotations per second = 360 degrees per second
    public float DPS = 60f;
    
    // Private variable to hold the Rigidbody component
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();
        
        // Freeze the rotation of the Rigidbody to prevent physics-based rotation
        rb.freezeRotation = true;    
    }

    // Update is called once per frame
    void Update()
    {

        // Calculate the amount of degree based on DPS and the time elapsed since the last frame
        float rotationAmount = DPS * Time.deltaTime;
        
        // Create a quaternion representing the rotation around the Y-axis
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
        
        // Apply the rotation to the Rigidbody
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
