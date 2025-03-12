using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    public Enums.DialogueTypingType typingType;

    [SerializeField] private List<DialogueEntry> _dialogueTexts = new List<DialogueEntry>();

    [SerializeField] private List<ListWrapper> _dialogues = new List<ListWrapper>();

    private DialogueController _dialogueController;
    private bool _isTalking = false;
    private Collider _collider;
    private PlayerInteractable _interactable;
    public bool IsTalking => _isTalking;

    private int _dialogueIndex = 0;
    public bool enableDialogueAdd = false;
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
            //_dialogueTexts
            _dialogueController.StartDialogue(_dialogues[_dialogueIndex].dialogues, typingType);
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
        Invoke(nameof(OnEnableCollider), 2f);
    }

    public void OnEnableCollider()
    {
        _collider.enabled = true;
        _isTalking = false;
    }
    public void OnDisableCollider()
    {
        _collider.enabled = false;
        _interactable.RemoveInteractable(gameObject);   
    }

    public void AddNewDialogue()
    {
        if(enableDialogueAdd)
        {
            _dialogueIndex++;
        }
    }

    public void EnableDialogueAdd()
    {
        enableDialogueAdd = true;
    }

    public void DisableDialogueAdd()
    {
        enableDialogueAdd = false;
    }

    public void SetInteractableParent(PlayerInteractable parent) => _interactable = parent;

    private void OnDestroy()
    {
        _dialogueController.DialogueEnded -= OnDialogueEnded;
    }
}
