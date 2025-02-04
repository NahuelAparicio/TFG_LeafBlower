using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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


    private void Awake()
    {
        _characterIcons = Resources.LoadAll<Sprite>(Constants.DIALOGUE_ICONS_PATH).ToList();
        _typeHandler = GetComponent<TypingHandler>();
        _dialogueHolder = transform.GetChild(0).gameObject;
        EnableInputs(); 
        HideDialogueBox();
    }

    public void StartDialogue(List<DialogueEntry> messages, Enums.DialogueTypingType t)
    {
        MusicManager.Instance.PlayDialogs();
        _typeHandler.SetTypingType(t);
        _currentDialogue.AddRange(messages);
        _dialogueHolder.SetActive(true);
        _icon.sprite = _characterIcons[(int)messages[_indexDialogue].character];
        _typeHandler.ShowMessage(_currentDialogue[_indexDialogue].text);
        DialogueStarted?.Invoke();
    }

    // Temporal, Fade/Effect should be added to image and text (?)
    private void HideDialogueBox()
    {
        MusicManager.Instance.PlayExplorationMusic();
        _currentDialogue.Clear();
        _typeHandler.ResetTypingType();
        _typeHandler.ResetText();
        _indexDialogue = 0;
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
        if(_indexDialogue == 0 && _currentDialogue.Count == 1)
        {
            HideDialogueBox();
        }
        else if(_currentDialogue.Count > 1)
        {
            if (_currentDialogue.Count - 1 >= _indexDialogue)
            {
                _icon.sprite = _characterIcons[(int)_currentDialogue[_indexDialogue].character];
                _typeHandler.ShowMessage(_currentDialogue[_indexDialogue].text);
                _currentDialogue[_indexDialogue].Invoke();
                _indexDialogue++;
            }
            else
            {
                HideDialogueBox();
            }
        }       
    }
    #endregion

}
