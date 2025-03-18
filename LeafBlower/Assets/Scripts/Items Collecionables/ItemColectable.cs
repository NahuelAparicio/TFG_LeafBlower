using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemColectable : MonoBehaviour
{
    public ItemData data;

    private void OnTriggerEnter(Collider other)
    {
        OnCollect();
    }

    protected virtual void OnCollect()
    {
        PickUpEffect();
    }

    protected virtual void PickUpEffect()
    {
        Destroy(gameObject);
    }
}
