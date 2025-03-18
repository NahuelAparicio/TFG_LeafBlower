using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class BlobShadow : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform player;
    [SerializeField] DecalProjector decal;
    [SerializeField] LayerMask targetRaycastLayers;

    void Awake()
    {
        if (player == null)
            player = transform.parent;

        if (decal == null)
        {
            decal = GetComponent<DecalProjector>();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool hasHit = Physics.Raycast(player.position + Vector3.up * 0.5f, -Vector3.up, out RaycastHit hit, 10000, targetRaycastLayers);
        decal.enabled = hasHit;
        if (hasHit)
        {
            transform.position = hit.point;
        }     
    }
}
