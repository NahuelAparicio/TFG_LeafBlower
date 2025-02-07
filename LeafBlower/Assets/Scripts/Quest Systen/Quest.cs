using UnityEngine;

public class Quest
{
    public QuestData data;
    public Enums.QuestState state;

    private int _currentQuestStepIndex;

    public Quest(QuestData data)
    {
        this.data = data;
        state = Enums.QuestState.Locked;
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
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(data.id);
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
