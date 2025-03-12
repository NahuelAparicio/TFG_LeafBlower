using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

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
    public GameObject minimapa;

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

        foreach (FadeImage fadeImg in _fade)
        {
            fadeImg.OnFadeIn();
        }
        foreach (FadeTextMeshPro fadeTxt in _texts)
        {
            fadeTxt.OnFadeIn();
        }
        foreach (FadeImage fadeImg in _fadeUI)
        {
            fadeImg.OnFadeOut();
        }
        foreach (FadeTextMeshPro fadeTxt in _textsUI)
        {
            fadeTxt.OnFadeOut();
        }
        minimapa.SetActive(false);
        _typeHandler.SetTypingType(t);

        _currentDialogue.Clear();
        _currentDialogue.AddRange(messages);

        _icon.sprite = _characterIcons[(int)messages[_indexDialogue].character];
        _typeHandler.ShowMessage(_currentDialogue[_indexDialogue].text);

        DialogueStarted?.Invoke();
    }

    private void HideDialogueBox()
    {
        foreach (FadeImage fadeImg in _fade)
        {
            fadeImg.OnFadeOut();
        }
        foreach (FadeTextMeshPro fadeTxt in _texts)
        {
            fadeTxt.OnFadeOut();
        }
        foreach (FadeImage fadeImg in _fadeUI)
        {
            if (fadeImg.isActiveAndEnabled)
            {
                fadeImg.OnFadeIn();
            }
        }
        foreach (FadeTextMeshPro fadeTxt in _textsUI)
        {
                fadeTxt.OnFadeIn();
        }
            minimapa.SetActive(true);

        Invoke(nameof(EndDialogue), _fade[0].fadeDuration);
    }

    private void EndDialogue()
    {
        DialogueEnded?.Invoke();
        //MusicManager.Instance.PlayExplorationMusic();
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
    private float _interactCooldown = 0.2f; // Cooldown time
    private void NextDialogue_performed(InputAction.CallbackContext context)
    {
        if (_currentDialogue.Count <= 0 || Time.time < _nextInteractTime) return;
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
        _nextInteractTime = Time.time + _interactCooldown;

    }

    private void PlayDialogueSound()
    {
        RuntimeManager.PlayOneShot("event:/Dialogs/Dialog_Start");
    }
    #endregion
}