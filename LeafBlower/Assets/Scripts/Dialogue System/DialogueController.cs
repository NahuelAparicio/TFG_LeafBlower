using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    private TypingHandler _typeHandler;

    [SerializeField] internal GameObject _dialogueHolder;

    private List<string> _currentDialogue = new List<string>();
    private int _indexDialogue = 0;

    private PlayerInputsActions _actions;
    public event System.Action DialogueStated;
    public event System.Action DialogueEnded;

    private void Awake()
    {
        _typeHandler = GetComponent<TypingHandler>();
        _dialogueHolder = transform.GetChild(0).gameObject;
        EnableInputs(); 
        HideDialogueBox();
    }

    public void StartDialogue(List<string> messages, Enums.DialogueTypingType t)
    {
        _typeHandler.SetTypingType(t);

        _currentDialogue.AddRange(messages);
        _dialogueHolder.SetActive(true);
        _typeHandler.ShowMessage(_currentDialogue[0]);

        DialogueStated?.Invoke();
    }

    // Temporal, Fade/Effect should be added to image and text (?)
    private void HideDialogueBox()
    {
        _typeHandler.ResetTypingType();
        _typeHandler.ResetText();
        _indexDialogue = 0;
        _currentDialogue.Clear();
        _dialogueHolder.SetActive(false);

        DialogueEnded?.Invoke();
    }

    #region Enable and Handle Inputs
    private void EnableInputs()
    {
        _actions = new PlayerInputsActions();
        _actions.Dialogue.Enable();
        _actions.Dialogue.NextDialogue.performed += NextDialogue_performed;
    }
    private void NextDialogue_performed(InputAction.CallbackContext context)
    {
        if (_currentDialogue.Count - 1 > _indexDialogue)
        {
            _indexDialogue++;
            _typeHandler.ShowMessage(_currentDialogue[_indexDialogue]);
        }
        else
        {
            HideDialogueBox();
        }
    }
    #endregion

}
