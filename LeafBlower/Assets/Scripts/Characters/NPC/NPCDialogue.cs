using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    public Enums.DialogueTypingType typingType;

    [SerializeField] private List<DialogueEntry> _dialogueTexts = new List<DialogueEntry>();

    private DialogueController _dialogueController;
    private bool _isTalking = false;
    private Collider _collider;
    private PlayerInteractable _interactable;
    public bool IsTalking => _isTalking;

    private void Awake()
    {
        _isTalking = false;
        _collider = GetComponent<Collider>();
        _dialogueController = FindObjectOfType<DialogueController>();
        _dialogueController.DialogueEnded += OnDialogueEnded;
    }

    private void ShowDialogue()
    {
        if(_dialogueController)
        {
            _dialogueController.StartDialogue(_dialogueTexts, typingType);
            OnDisableCollider();
            _isTalking = true;
        }
    }

    public void OnInteract()
    {
        if(!_isTalking)
        {
            ShowDialogue();
        }
    }

    private void OnDialogueEnded()
    {
        _isTalking = false;
        GameEventManager.Instance.cameraEvents.ResetZoom();
        Invoke(nameof(OnEnableCollider), 2f);
    }

    public void OnEnableCollider() => _collider.enabled = true;
    public void OnDisableCollider()
    {
        _collider.enabled = false;
        _interactable.RemoveInteractable(gameObject);   
    }

    public void SetInteractableParent(PlayerInteractable parent) => _interactable = parent;

    private void OnDestroy()
    {
        _dialogueController.DialogueEnded -= OnDialogueEnded;
    }
}
