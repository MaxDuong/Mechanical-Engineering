using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPosition : MonoBehaviour
{
    public GameObject[] targetObjects; // Assign the object whose position you want to display
    public TMP_Text[] positionTexts;       // Assign the Text UI element in the Inspector

    void Update()
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null && positionTexts[i] != null)
            {
                // Get the position of the target object
                Vector3 position = targetObjects[i].transform.position;
                string objectName = targetObjects[i].name;

                // Update the text to display the position
                positionTexts[i].text = $"{objectName}: x={position.x:F2}, Y={position.y:F2}, Z={position.z:F2}";
            }
        }
    }
}
