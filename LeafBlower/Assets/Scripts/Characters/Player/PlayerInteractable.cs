using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    private List<GameObject> _touchingInteractables = new List<GameObject>();

    public bool canInteract;

    // Saves and deletes interacable objects of the list
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            if(other.GetComponent<IInteractable>() != null)
            {
                canInteract = true;
            }
            if(!_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Add(other.gameObject);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            canInteract = false;
            if(_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Remove(other.gameObject);
            }
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
