using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct DialogueEntry
{
    public Enums.CharacterNames character;
    [TextArea]public string text;
    public UnityEvent onDialogueEvent;
    public DialogueEntry(Enums.CharacterNames name, string entry, UnityEvent _onDialogueEvent)
    {
        character = name; text = entry; onDialogueEvent = _onDialogueEvent; 
    }

    public void Invoke() => onDialogueEvent?.Invoke();
}