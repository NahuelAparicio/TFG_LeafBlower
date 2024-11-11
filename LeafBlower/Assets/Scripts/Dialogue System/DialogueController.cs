using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueHolder;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private PlayerInputsActions _actions;

    private List<string> _currentDialogue = new List<string>();
    private int _indexDialogue = 0;

    [SerializeField] private int _maxCharsPerDialogue = 50;

    public event System.Action DialogueStated;
    public event System.Action DialogueEnded;

    private void Awake()
    {
        _dialogueHolder = transform.GetChild(0).gameObject;
        _dialogueText = _dialogueHolder.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _actions = new PlayerInputsActions();
        _actions.Dialogue.Enable();
        _actions.Dialogue.NextDialogue.performed += NextDialogue_performed;
        HideDialogueBox();
    }

    public void StartDialogue(List<string> messages)
    {
        _currentDialogue.AddRange(messages);
        _dialogueHolder.SetActive(true);
        ShowMessage(_currentDialogue[0]);
        DialogueStated?.Invoke();
    }

    //Given a list of messages (dialogues in order)
    //It will show the dialogues in screen i guess
    private void ShowMessage(string text)
    {
        _dialogueText.text = text;
    }

    // Temporal, Fade/Effect should be added to image and text (?)
    private void HideDialogueBox()
    {
        _indexDialogue = 0;
        _currentDialogue.Clear();
        _dialogueHolder.SetActive(false);
        _dialogueText.text = "";
        DialogueEnded?.Invoke();
    }

    private void NextDialogue_performed(InputAction.CallbackContext context)
    {
        if(_currentDialogue.Count - 1 > _indexDialogue)
        {
            _indexDialogue++;
            ShowMessage(_currentDialogue[_indexDialogue]);
        }
        else
        {
            HideDialogueBox();
        }
    }
}
