using UnityEngine;

public class BeltMovement : MonoBehaviour {
    public float speed = 50.0f; // Speed at which the texture moves
    public StartButton script; // Reference to the StartButton script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update(){
        
        if(script.start){
            // Calculate the texture offset based on the elapsed time and speed
            float Transition = Time.time * speed;
            
            // Apply the calculated offset to the material's main texture
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Transition, 0);
        }
    }
}
