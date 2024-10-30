using UnityEngine;

public class TestInteract : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        Debug.Log("Interacted Tested");
    }
}
