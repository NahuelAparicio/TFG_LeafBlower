using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractable : MonoBehaviour
{
    private List<GameObject> _touchingInteractables = new List<GameObject>();
    public bool canInteract;

    private void Update()
    {
        if (_touchingInteractables.Count <= 0)
        {
            canInteract = false;
        }
    }

    // Saves and deletes interacable objects so you can interact with them
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                canInteract = true;

                InteractUIManager uiManager = other.GetComponentInChildren<InteractUIManager>();
                if (uiManager != null)
                {
                    uiManager.SetIconVisibility(true);
                }

                interactable.SetInteractableParent(this);
            }
            if (!_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Add(other.gameObject);
            }
        }
    }

    public void RemoveInteractable(GameObject go)
    {
        if (_touchingInteractables.Contains(go))
        {
            if (_touchingInteractables.Count == 1)
            {
                InteractUIManager uiManager = go.GetComponentInChildren<InteractUIManager>();
                if (uiManager != null)
                {
                    uiManager.SetIconVisibility(false);
                }
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            canInteract = false;
            RemoveInteractable(other.gameObject);
        }
    }

    public void ClearInteractables() => _touchingInteractables.Clear();

    // On Interact Button Performed checks interaction posibilities and intercts if it's possible
    public void InteractPerformed()
    {
        if (!canInteract) return;

        List<GameObject> interactablesCopy = new List<GameObject>(_touchingInteractables);

        foreach (var interactable in interactablesCopy)
        {
            if (interactable != null)
            {
                var interaction = interactable.GetComponent<IInteractable>();
                if (interaction == null) return;
                interaction.OnInteract();
            }
        }
    }
}
