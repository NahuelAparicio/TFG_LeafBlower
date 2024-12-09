using UnityEngine;

public class TestInteract : MonoBehaviour, IInteractable
{
    public void OnDisableCollider()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnableCollider()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        Debug.Log("Interacted Tested");
    }

    public void SetInteractableParent(PlayerInteractable parent)
    {
        throw new System.NotImplementedException();
    }
}
