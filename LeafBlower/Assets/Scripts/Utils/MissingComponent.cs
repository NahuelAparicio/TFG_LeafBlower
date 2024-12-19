using UnityEditor;
using UnityEngine;

public class MissingComponent : MonoBehaviour
{
    [MenuItem("Tools/Find Missing Components In Scene")]
    static void FindMissingComponents()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int missingCount = 0;

        foreach (GameObject go in allObjects)
        {
            Component[] components = go.GetComponents<Component>();

            foreach (Component c in components)
            {
                if (c == null)
                {
                    Debug.LogError("Missing Component in GameObject: " + go.name, go);
                    missingCount++;
                }
            }
        }

        Debug.Log("Total missing components found: " + missingCount);
    }
}
