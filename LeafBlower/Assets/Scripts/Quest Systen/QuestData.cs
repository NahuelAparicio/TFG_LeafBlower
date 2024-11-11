using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Quests")]
public class QuestData : ScriptableObject
{
    public int id;
    public string qName;
    [TextArea] public string description;
    public Sprite icon;
}
