using UnityEngine;

[System.Serializable]
public struct DialogueEntry
{
    public Enums.CharacterNames character;
    public string text;

    public DialogueEntry(Enums.CharacterNames name, string entry)
    {
        character = name; text = entry;
    }
}