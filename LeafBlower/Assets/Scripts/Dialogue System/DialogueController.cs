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

    private void Awake()
    {
        _dialogueHolder = transform.GetChild(0).gameObject;
        _dialogueText = _dialogueHolder.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _actions = new PlayerInputsActions();
        _actions.Dialogue.Enable();
        _actions.Dialogue.NextDialogue.performed += NextDialogue_performed;
    }

    void Update()
    {
        
    }
    //Given a list of messages (dialogues in order)
    //It will show the dialogues in screen i guess
    public void ShowMessage(List<string> messages)
    {
        _currentDialogue.AddRange(messages);
    }

    //Shows a singles text message, if its to long it will be divided in differen "dialogue boxes"
    public void ShowMessage(string message)
    {
        _currentDialogue.Add(message);

        _dialogueHolder.SetActive(true);
        _dialogueText.text = message;
    }

    private void NextDialogue_performed(InputAction.CallbackContext context)
    {
        if(_currentDialogue.Count > 0)
        {
            _indexDialogue++;
            ShowMessage(_currentDialogue[_indexDialogue]);
        }
    }
}
