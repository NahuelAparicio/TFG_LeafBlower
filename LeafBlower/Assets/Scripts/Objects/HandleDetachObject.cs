using UnityEngine;

public class HandleDetachObject : MonoBehaviour
{
    private LayerMask ground;

    private void Awake()
    {
        ground = LayerMask.NameToLayer("Ground");

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == ground && other.CompareTag("IsWall"))
        {
            GameEventManager.Instance.playerEvents.DetachObject();
        }
    }
}
