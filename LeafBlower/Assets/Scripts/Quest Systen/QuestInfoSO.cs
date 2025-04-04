using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Quests")]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General info: ")]
    public string qName;
    [TextArea] public string description;
    public Sprite icon;


    [Header("Requirements :")]
    public QuestInfoSO[] questPreequisits;

    [Header("Steps: ")]
    public GameObject[] questStepPrefabs;

    [Header("Rewards: ")]
    public int goldReward;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
