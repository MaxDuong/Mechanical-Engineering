using UnityEngine;

public class BeltAnimation : MonoBehaviour {
    public float speed = 50.0f; // Speed at which the texture moves
    public StartButtonControl script; // Reference to the StartButton script
    public ConveyorSpeedControl SpeedControl;  // Reference to the ConveyorSpeedControl script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update(){
        
        if(script.start){
            // Calculate the texture offset based on the elapsed time and speed
            float Transition = Time.time * SpeedControl.BeltSpeed;
            
            // Apply the calculated offset to the material's main texture
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Transition, 0);
        }
    }
}
