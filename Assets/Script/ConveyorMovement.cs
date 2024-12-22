using UnityEngine;

public class ConveyorMovement : MonoBehaviour
{
    public float speed = 0.1f; // Speed of the conveyor belt
    Rigidbody conv; // Rigidbody component of the Conveyor Belt

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        conv = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the Conveyor Belt
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = conv.position; // Get the position of the Conveyor Belt
        conv.position = conv.position + Vector3.right * speed * Time.deltaTime; // Move the Conveyor Belt to the left
        conv.MovePosition(pos); // Update the position of the Conveyor Belt
    }
}
