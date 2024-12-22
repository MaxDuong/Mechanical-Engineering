using UnityEngine;

public class ConveyorMaterialMotion : MonoBehaviour
{
    public float speed = -0.135f; // Speed at which the texture moves

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Debug logs to print the initial position of the GameObject
        Debug.Log("Start");
        Debug.Log(transform.position);
        Debug.Log(transform.position.x);
        Debug.Log(transform.position.y);
        Debug.Log(transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the texture offset based on the elapsed time and speed
        float Transition = Time.time * speed;
        
        // Apply the calculated offset to the material's main texture
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Transition);
    }
}