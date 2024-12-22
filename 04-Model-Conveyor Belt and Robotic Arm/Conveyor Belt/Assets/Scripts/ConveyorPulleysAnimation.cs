using UnityEngine;

public class ConveyorPulleysAnimation : MonoBehaviour {
    public float speed = 50.0f;
    public GameObject FrontPulley;
    public GameObject BackPulley;
    public StartButtonControl script;  // Reference to the StartButton script
    public ConveyorSpeedControl SpeedControl;  // Reference to the ConveyorSpeedControl script
    // Start is called once before the first execution of Update after the MonoBehavior is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(script.start){
            FrontPulley.transform.Rotate(Vector3.up * SpeedControl.ConveyorPulleysSpeed * Time.deltaTime);
            BackPulley.transform.Rotate(Vector3.up * SpeedControl.ConveyorPulleysSpeed * Time.deltaTime);
        }
    }
}
