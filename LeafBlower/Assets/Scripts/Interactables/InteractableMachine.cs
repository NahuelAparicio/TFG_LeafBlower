using UnityEngine;

public class InteractableMachine : MonoBehaviour,IInteractable
{
    public GameObject prefab;
    public Transform instantiationPoint;
    private PlayerInteractable _interactable;
    [SerializeField]private Collider _collider;
    public GameObject instantiatedPrefab;

    public GameObject InteractableIcon;

    private void Awake()
    {
            InteractableIcon.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            InteractableIcon.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractableIcon.SetActive(false);
        }
    }
    public void InstatiatePrefab()
    {
        if(instantiatedPrefab != null)
            Destroy(instantiatedPrefab);

        instantiatedPrefab = Instantiate(prefab, instantiationPoint.position, Quaternion.identity);
    }

    public void OnDisableCollider()
    {
        _collider.enabled = false;
        _interactable.RemoveInteractable(gameObject);
    }

    public void OnEnableCollider()
    {
        _collider.enabled = true;

    }

    public void OnInteract()
    {
        InstatiatePrefab();
    }

    public void SetInteractableParent(PlayerInteractable parent)
    {
        _interactable = parent;
    }
}
