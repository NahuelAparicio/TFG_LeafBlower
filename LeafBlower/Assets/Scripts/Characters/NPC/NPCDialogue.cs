using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private List<string> _dialogueTexts = new List<string>();
    public Enums.DialogueTypingType typingType;

    private DialogueController _dialogueController;
    private bool _isTalking = false;
    public bool IsTalking => _isTalking;

    private void Awake()
    {
        _isTalking = false;
        _dialogueController = FindObjectOfType<DialogueController>();
        _dialogueController.DialogueEnded += OnDialogueEnded;
    }

    private void ShowDialogue()
    {
        if(_dialogueController)
        {
            _dialogueController.StartDialogue(_dialogueTexts, typingType);
            _isTalking = true;
        }
    }

    public void OnInteract()
    {
        if(!_isTalking)
            ShowDialogue();
    }

    private void OnDialogueEnded()
    {
        _isTalking = false;
        //Can Move? Etc?
    }
}
