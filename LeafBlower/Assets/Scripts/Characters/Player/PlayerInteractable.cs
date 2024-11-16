using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    private List<GameObject> _touchingInteractables = new List<GameObject>();
    public bool canInteract;

    // Saves and deletes interacable objects so you can interact with them
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
            if(interactable != null)
            {
                canInteract = true;
                interactable.SetInteractableParent(this);
            }
            if(!_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Add(other.gameObject);
            }

        }
    }

    public void RemoveInteractable(GameObject go)
    {
        if(_touchingInteractables.Contains(go))
        {
            if(_touchingInteractables.Count == 1)
            {
                _touchingInteractables.Clear();
            }
            else
            {
                _touchingInteractables.Remove(go);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            canInteract = false;
            RemoveInteractable(other.gameObject);
        }
    }

    public void ClearInteractables() => _touchingInteractables.Clear();

    // On Interact Button Performed checks interaction posibilities and intercts if it's possible
    public void InteractPerformed()
    {
        foreach(var interactable in _touchingInteractables)
        {
            if(interactable != null)
            {
                var interaction = interactable.GetComponent<IInteractable>();
                if (interaction == null) return;
                interaction.OnInteract();
            }
        }
    }
}
