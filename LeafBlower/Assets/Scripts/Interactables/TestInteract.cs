using UnityEngine;

public class TestInteract : MonoBehaviour, IInteractable
{
    public void OnDisableCollider()
    {

    }

    public void OnEnableCollider()
    {


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
