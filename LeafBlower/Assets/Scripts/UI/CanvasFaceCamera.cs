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
            Debug.LogWarning("No se encontró la cámara principal. Asegúrate de que la cámara tenga el tag 'MainCamera'.");
        }
    }
}
