using UnityEngine;

public class CaptureImageWithObjectPosition : MonoBehaviour {
    public Camera mainCamera; // Reference to the camera
    public GameObject targetObject; // The object whose position you want to determine
    public GameObject baseOrigin; // Reference to BaseOrigin(2)
    public KeyCode captureKey = KeyCode.P; // Press 'P' to take a screenshot

    void Update() {
        if (Input.GetKeyDown(captureKey)) {
            TakeScreenshotAndLogObjectPosition();
        }
    }

    public Vector3 ConvertLocalToWorld(Vector3 localPosition){
        // Simply use TransformPoint to convert local position to world position
        return transform.TransformPoint(localPosition);
    }

    void TakeScreenshotAndLogObjectPosition() {
        // Step 2: Take a screenshot
        string fileName = $"Screenshot";
        string filePath = $"{Application.dataPath}/Images/{fileName}_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";

        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log("Screenshot saved to: " + filePath);

        // // 1: Get the object's local position
        // Vector3 localPosition = targetObject.transform.localPosition;
        // Debug.Log("Object Local Position: " + localPosition);

        // // 2: Get the object's world position
        // Vector3 worldPosition = targetObject.transform.position;
        // Debug.Log("Object World Position: " + worldPosition);

        // Vector3 worldPosition_conversion = ConvertLocalToWorld(localPosition);
        // Debug.Log($"World Position Conversion: {worldPosition}");

        // // 3: Convert world position to screen position
        // Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        // Debug.Log("Object Screen Position: " + screenPosition);
        
    }
}
