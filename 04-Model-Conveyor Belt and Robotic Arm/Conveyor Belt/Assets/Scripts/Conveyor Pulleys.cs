using UnityEngine;

public class ConveyorPulleys : MonoBehaviour {
    public float speed = 50.0f;
    public GameObject FrontPulley;
    public GameObject BackPulley;
    public bool start = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(start){
            FrontPulley.transform.Rotate(Vector3.up * speed * Time.deltaTime);
            BackPulley.transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }
    }
}
