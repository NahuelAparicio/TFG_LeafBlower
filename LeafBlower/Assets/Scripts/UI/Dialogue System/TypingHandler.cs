using TMPro;
using UnityEngine;
using System.Collections;
using FMODUnity; // Asegúrate de tener esta línea

public class TypingHandler : MonoBehaviour
{
    private DialogueController _dialogue;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private bool _isDialoguePrinted = false;

    private Enums.DialogueTypingType _typingType;

    private string _currentMessage;

    public bool IsDialoguePrinted => _isDialoguePrinted;

    private Coroutine _typingCoroutine;

    public float timeBetweenLetters;

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

    private void ShowMessageNoEffect()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }

        _dialogueText.text = _currentMessage;
        _isDialoguePrinted = true;
    }

    private void ShowMessageTypeMachine()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        _typingCoroutine = StartCoroutine(TypeMachineMessage());
    }

    private IEnumerator TypeMachineMessage()
    {
        _isDialoguePrinted = false;
        _dialogueText.text = "";

        foreach (char letter in _currentMessage)
        {
            _dialogueText.text += letter;

            if (!char.IsWhiteSpace(letter))
            {
                RuntimeManager.PlayOneShot("event:/UI/Typing");
            }

            yield return new WaitForSeconds(timeBetweenLetters);
        }

        _isDialoguePrinted = true;
        _typingCoroutine = null;
    }

    public void FinishTypingImmediately()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }

        _dialogueText.text = _currentMessage;
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
