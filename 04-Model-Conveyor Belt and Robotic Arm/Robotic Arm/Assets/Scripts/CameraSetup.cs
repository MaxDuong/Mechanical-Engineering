using UnityEngine;
using System;

[Serializable]
public class CameraParameters
{
    public float fov;
    public float aspect;
    public Vector3 position;
    public Vector3 rotation;
}

public class CameraSetup : MonoBehaviour
{
    public Camera mainCamera;
    
    void Start()
    {
        // Set camera parameters
        mainCamera.fieldOfView = 60f;
        mainCamera.nearClipPlane = 0.3f;
        mainCamera.farClipPlane = 1000f;

        SaveCameraParameters();
    }
    
    void SaveCameraParameters()
    {
        CameraParameters parameters = new CameraParameters
        {
            fov = mainCamera.fieldOfView,
            aspect = mainCamera.aspect,
            position = mainCamera.transform.localPosition,
            rotation = mainCamera.transform.localEulerAngles
        };

        // string json = JsonUtility.ToJson(parameters, true);  // true for pretty print
        // string fullPath = System.IO.Path.Combine(Application.dataPath, "camera_params.json");
        // System.IO.File.WriteAllText(fullPath, json);
        // Debug.Log("Camera parameters saved to: " + fullPath);
        // Debug.Log("JSON content: " + json);  // To verify the content
    }
}