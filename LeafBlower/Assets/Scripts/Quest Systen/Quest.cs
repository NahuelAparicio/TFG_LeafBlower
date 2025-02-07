using UnityEngine;

public class Quest
{
    public QuestData data;
    private Enums.QuestState _state;
    public Enums.QuestState State => _state;

    private int _currentQuestStepIndex;

    public Quest(QuestData data)
    {
        this.data = data;
        _state = Enums.QuestState.Locked;
        _currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool CurrentStepExists() => _currentQuestStepIndex < data.questStepPrefabs.Length;

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if(questStepPrefab != null)
        {
            Object.Instantiate<GameObject>(questStepPrefab, parentTransform);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questPrefab = null;
        if(CurrentStepExists())
        {
            questPrefab = data.questStepPrefabs[_currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Out of range step prefab" + "No current step for Quest id" + data.id + ", stepindex " + _currentQuestStepIndex);
        }

        return questPrefab;
    }
}
