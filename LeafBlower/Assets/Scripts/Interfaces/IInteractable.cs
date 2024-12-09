using UnityEngine;

public interface IInteractable 
{
    public void OnInteract();
    public void SetInteractableParent(PlayerInteractable parent);
    public void OnEnableCollider();
    public void OnDisableCollider();
}
