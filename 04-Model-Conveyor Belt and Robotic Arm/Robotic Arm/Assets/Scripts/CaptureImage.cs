using UnityEngine;

public class CaptureImage : MonoBehaviour {
    public string fileName = "Screenshot";
    public KeyCode captureKey = KeyCode.P; // Press 'P' to take a screenshot

    void Update() {
        if (Input.GetKeyDown(captureKey)) {
            TakeScreenshot();
        }
    }

    void TakeScreenshot(){
        // string filePath = "C:\Personal Projects\Mechanical-Engineering\04-Model-Conveyor Belt and Robotic Arm\Robotic Arm 2\Robotic Arm\Assets\Images\abc.png';
        string filePath = $"{Application.dataPath}/Images/{fileName}_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log($"Screenshot saved to: {filePath}");
    }
    
}
