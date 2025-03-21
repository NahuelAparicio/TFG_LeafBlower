using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    public Enums.DialogueTypingType typingType;

    [SerializeField] private List<ListWrapper> _dialogues = new List<ListWrapper>();

    private DialogueController _dialogueController;
    private bool _isTalking = false;
    private Collider _collider;
    private PlayerInteractable _interactable;
    public bool IsTalking => _isTalking;

    private int _dialogueIndex = 0;
    public bool enableDialogueAdd = false;

    private InteractUIManager _uiManager; // Donde esté el script que maneja la visibilidad de la interfaz de interacción

    private void Awake()
    {
        _isTalking = false;
        _collider = GetComponent<Collider>();
        _dialogueController = FindObjectOfType<DialogueController>();
        _dialogueController.DialogueEnded += OnDialogueEnded;

        _uiManager = GetComponent<InteractUIManager>();
        LoadData();
    }

    private void ShowDialogue()
    {
        if (_dialogueController)
        {
            _dialogueController.StartDialogue(_dialogues[_dialogueIndex].dialogues, typingType);
            OnDisableCollider();
            _isTalking = true;
        }
    }

    public void OnInteract()
    {
        if (!_isTalking)
        {
            ShowDialogue();
            _uiManager.HideIcon();
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
        _uiManager.SetIconVisibility(true); //aqui molaria hacer una animación de bounce por codigo
    }

    public void OnDisableCollider()
    {
        _collider.enabled = false;
        _interactable.RemoveInteractable(gameObject);

    }

    public void AddNewDialogue()
    {
        if (enableDialogueAdd)
        {
            if (_dialogues.Count - 1 == _dialogueIndex) return;
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

    private void OnDisable()
    {
        SaveData();
    }

    private void SaveData()
    {
        string key = "IndexDialogue_" + gameObject.name; // Clave única para cada NPC
        PlayerPrefs.SetInt(key, _dialogueIndex);
    }

    private void LoadData()
    {
        string key = "IndexDialogue_" + gameObject.name; // Recupera la clave correcta
        _dialogueIndex = PlayerPrefs.GetInt(key, 0); // Por defecto 0 si no se encuentra
    }
}
