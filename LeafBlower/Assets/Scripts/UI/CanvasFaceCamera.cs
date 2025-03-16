using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CanvasFaceCamera : MonoBehaviour
{
    void Start()
    {
        CopyCameraRotation();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            CopyCameraRotation();
        }
    }

    void CopyCameraRotation()
    {
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            Debug.LogWarning("No se encontr� la c�mara principal. Aseg�rate de que la c�mara tenga el tag 'MainCamera'.");
        }
    }
}
