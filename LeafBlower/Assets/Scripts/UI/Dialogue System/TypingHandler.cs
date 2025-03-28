using TMPro;
using UnityEngine;
using System.Collections;

public class TypingHandler : MonoBehaviour
{
    private DialogueController _dialogue;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private bool _isDialoguePrinted = false;

    private Enums.DialogueTypingType _typingType;

    private string _currentMessage;

    public bool IsDialoguePrinted => _isDialoguePrinted;

    private void Awake()
    {
        _dialogue = GetComponent<DialogueController>();
    }

    private void Start()
    {
        _dialogueText = _dialogue._dialogueHolder.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ShowMessage(string message)
    {
        _currentMessage = message;
        switch (_typingType)
        {
            case Enums.DialogueTypingType.NoEffect:
                ShowMessageNoEffect();
                break;
            case Enums.DialogueTypingType.TypingMachine:
                ShowMessageTypeMachine();
                break;
            default:
                break;
        }
    }

    //Given a list of messages (dialogues in order)
    //It will show the dialogues in screen i guess
    private void ShowMessageNoEffect()
    {
        _dialogueText.text = _currentMessage;
        _isDialoguePrinted = true;
    }

    private void ShowMessageTypeMachine()
    {
        StartCoroutine(TypeMachineMessage());
    }

    private IEnumerator TypeMachineMessage()
    {
        _isDialoguePrinted = false;
        _dialogueText.text = "";
        foreach (char letter in _currentMessage)
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        _isDialoguePrinted = true;
    }

    public void SetTypingType(Enums.DialogueTypingType type) 
    {
        if (_typingType == type) return;

        _typingType = type; 
    }
    public void ResetTypingType() => _typingType = Enums.DialogueTypingType.NoEffect;
    public void ResetText() => _dialogueText.text = "";
}
