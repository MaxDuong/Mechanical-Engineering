using UnityEngine;

public class ColliderTest2 : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
    }

    // This method is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other) {
        // if (other.gameObject.name == "AnimationEE") {
        //     Debug.Log("Hit Object");
        // }
        Debug.Log("Hit Object 2");
    }
}
