using System;
using System.Collections.Generic;
using UnityEngine;

public class TransparentDetector : MonoBehaviour
{
    private static int PosID = Shader.PropertyToID("_PlayerPos");
    private static int IsTransparentID = Shader.PropertyToID("_IsTransparent");

    private List<Renderer> affectedRenderers = new List<Renderer>();
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        DetectObjects();
    }

    private void DetectObjects()
    {
        Vector3 direction = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) - _mainCamera.transform.position;
        float distance = direction.magnitude;

        Ray ray = new Ray(Camera.main.transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance);

        if (hits.Length == 0)
        {
            // No objects detected, restore previous state and exit early
            RestoreTransparency(new HashSet<Renderer>());
            return;
        }

        HashSet<Renderer> newAffectedRenderers = new HashSet<Renderer>();
        bool hasTransparentObjects = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out Renderer renderer) && renderer.gameObject.tag != "Player")
            {
                if (!originalMaterials.ContainsKey(renderer))
                {
                    originalMaterials[renderer] = renderer.materials;
                }

                foreach (Material mat in renderer.materials)
                {
                    mat.SetInt(IsTransparentID, 1);
                    SetPlayerPosInMaterial(mat);
                }

                newAffectedRenderers.Add(renderer);
                hasTransparentObjects = true;
            }
        }

        // If no new transparent objects were detected, avoid redundant calls
        if (hasTransparentObjects)
        {
            RestoreTransparency(newAffectedRenderers);
        }
    }

    private void RestoreTransparency(HashSet<Renderer> newAffectedRenderers)
    {
        List<Renderer> toRemove = new List<Renderer>();

        foreach (Renderer renderer in affectedRenderers)
        {
            if (!newAffectedRenderers.Contains(renderer))
            {
                if (originalMaterials.ContainsKey(renderer))
                {
                    foreach (Material mat in renderer.materials)
                    {
                        mat.SetInt(IsTransparentID, 0);
                    }
                    originalMaterials.Remove(renderer);
                }
                toRemove.Add(renderer);
            }
        }

        foreach (Renderer renderer in toRemove)
        {
            affectedRenderers.Remove(renderer);
        }

        affectedRenderers = new List<Renderer>(newAffectedRenderers);
    }

    private void SetPlayerPosInMaterial(Material mat)
    {
        var view = Camera.main.WorldToViewportPoint(transform.position);
        mat.SetVector(PosID, view);
    }
}
