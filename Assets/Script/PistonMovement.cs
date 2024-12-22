using UnityEngine;

public class PistonMovement : MonoBehaviour{
    private Rigidbody Piston; // Rigidbody component of the Piston
    public float speed = 1f; // Speed of the piston movement
    public Vector3 originalPos; // Original position of the Piston
    public Vector3 targetPos; // Target position
    public Transform targetObject; // Target object to move the Piston towards
    private bool MovingTowardsTarget = true; // Flag to track if the Piston is moving towards the target
    public bool start = false; // Flag to start the piston movement
    private Vector3 pos; // Position of the Piston

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        Piston = GetComponent<Rigidbody>(); // Get the Rigidbody component of the Piston
        
        // Store the original position of the Piston
        originalPos = Piston.position; 
        targetPos = targetObject.position;

        Debug.Log("Original Position " + originalPos);
        Debug.Log("Target Position " + targetPos);
    }

    // Update is called once per frame
    void Update(){
        if (start){
            if (MovingTowardsTarget){
                pos = Vector3.MoveTowards(Piston.position, targetPos, speed * Time.deltaTime);
                Piston.MovePosition(pos); // Update the position of the Piston

                if (Vector3.Distance(Piston.position, targetPos) < 0.001f){
                    MovingTowardsTarget = false;
                }
            }
            else{
                pos = Vector3.MoveTowards(Piston.position, originalPos, speed * Time.deltaTime);
                Piston.MovePosition(pos); // Update the position of the Piston

                if (Vector3.Distance(Piston.position, originalPos) < 0.001f){
                    MovingTowardsTarget = true;
                    // start = false; // Stop the movement after returning to the original position
                }
            }
        }
    }
}