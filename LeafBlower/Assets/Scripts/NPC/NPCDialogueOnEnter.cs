using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCDialogueOnEnter : MonoBehaviour 
{
    public Enums.DialogueTypingType typingType;

    [SerializeField] private List<ListWrapper> _dialogues = new List<ListWrapper>();

    private DialogueController _dialogueController;
    private bool _isTalking = false;
    private int _dialogueIndex = 0;

    private void Awake()
    {
        _isTalking = false;
        _dialogueController = FindObjectOfType<DialogueController>();
        _dialogueController.DialogueEnded += OnDialogueEnded;
        LoadData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!_isTalking)
        {
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        if (_dialogueController)
        {
            _dialogueController.StartDialogue(_dialogues[_dialogueIndex].dialogues, typingType);
            GetComponent<Collider>().enabled = false;
            _isTalking = true;
        }
    }
    public UnityEvent eventOnEnd;
    private void OnDialogueEnded()
    {
        _isTalking = false;
        eventOnEnd.Invoke();
    }

    public void AddNewDialogue()
    {
        if (_dialogues.Count - 1 == _dialogueIndex) return;
        _dialogueIndex++;
    }

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
        string key = "IndexDialogue_" + gameObject.name;
        PlayerPrefs.SetInt(key, _dialogueIndex);
    }

    private void LoadData()
    {
        string key = "IndexDialogue_" + gameObject.name;
        _dialogueIndex = PlayerPrefs.GetInt(key, 0);
    }
}
