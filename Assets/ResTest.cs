using UnityEngine;

public class ResTest : MonoBehaviour
{
    RectTransform rt;
    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;

        // Print the resolutions
        foreach (var res in resolutions)
        {
            Debug.Log(res.width + "x" + res.height + " : " + res.refreshRate);
        }
        
        
    }
}