using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMODUnity;

public class DialogueController : MonoBehaviour
{
    private TypingHandler _typeHandler;
    private List<Sprite> _characterIcons = new List<Sprite>();
    private List<DialogueEntry> _currentDialogue = new List<DialogueEntry>();
    private int _indexDialogue = 0;

    [SerializeField] internal GameObject _dialogueHolder;
    [SerializeField] private Image _icon;

    private PlayerInputsActions _actions;
    public event System.Action DialogueStarted;
    public event System.Action DialogueEnded;

    public FadeImage[] _fade;
    public FadeTextMeshPro[] _texts;
    public FadeImage[] _fadeUI;
    public FadeTextMeshPro[] _textsUI;

    private void Awake()
    {
        _characterIcons = Resources.LoadAll<Sprite>(Constants.DIALOGUE_ICONS_PATH).ToList();
        _typeHandler = GetComponent<TypingHandler>();
        _dialogueHolder = transform.GetChild(1).gameObject;
        EnableInputs();
        _dialogueHolder.SetActive(false);
    }

    public void StartDialogue(List<DialogueEntry> messages, Enums.DialogueTypingType t)
    {
        if (messages == null || messages.Count == 0) return;

        GameEventManager.Instance.cameraEvents.Zoom(15);
        _dialogueHolder.SetActive(true);
        _typeHandler.SetTypingType(t);

        _currentDialogue.Clear();
        _currentDialogue.AddRange(messages);

        // Mostrar primer mensaje
        _icon.sprite = _characterIcons[(int)_currentDialogue[0].character];
        _typeHandler.ShowMessage(_currentDialogue[0].text);
        _currentDialogue[0].Invoke();
        PlayDialogueSound();

        _indexDialogue = 1; // Avanzamos el índice tras el primer mensaje
        DialogueStarted?.Invoke();
    }

    private void HideDialogueBox()
    {
        //StartFadeIn();
        Invoke(nameof(EndDialogue), _fade[0].fadeDuration);
    }

    private void EndDialogue()
    {
        DialogueEnded?.Invoke();
        _currentDialogue.Clear();
        _typeHandler.ResetTypingType();
        _typeHandler.ResetText();
        _indexDialogue = 0;
        _dialogueHolder.SetActive(false);
    }

    #region Enable and Handle Inputs

    private void EnableInputs()
    {
        _actions = new PlayerInputsActions();
        _actions.Dialogue.Enable();
        _actions.Dialogue.NextDialogue.performed += NextDialogue_performed;
    }

    private float _nextInteractTime = 0f;
    private float _interactCooldown = 0.25f;

    private void NextDialogue_performed(InputAction.CallbackContext context)
    {
        if (_currentDialogue.Count <= 0 || Time.time < _nextInteractTime) return;

        if (!_typeHandler.IsDialoguePrinted)
        {
            _typeHandler.FinishTypingImmediately();
        }
        else
        {
            if (_indexDialogue < _currentDialogue.Count)
            {
                _icon.sprite = _characterIcons[(int)_currentDialogue[_indexDialogue].character];
                _typeHandler.ShowMessage(_currentDialogue[_indexDialogue].text);
                _currentDialogue[_indexDialogue].Invoke();
                PlayDialogueSound();
                _indexDialogue++;
            }
            else
            {
                GameEventManager.Instance.cameraEvents.ResetZoom();
                HideDialogueBox();
            }
        }

        _nextInteractTime = Time.time + _interactCooldown;
    }

    private void PlayDialogueSound()
    {
        RuntimeManager.PlayOneShot("event:/Dialogs/Dialog_Start");
    }

    #endregion
}