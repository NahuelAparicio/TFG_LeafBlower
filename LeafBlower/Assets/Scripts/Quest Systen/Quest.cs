using UnityEngine;
using TMPro;
using static Enums;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;

    private int _currentQuestStepIndex;
    private QuestStepState[] _questStepStates;
    public TextMeshProUGUI text;

    public Quest(QuestInfoSO _data)
    {
        info = _data;
        state = QuestState.RequirementNotMet;
        _currentQuestStepIndex = 0;

        _questStepStates = new QuestStepState[info.questStepPrefabs.Length];

        for (int i = 0; i < _questStepStates.Length; i++)
        {
            _questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO _data, QuestState _state, int currentQuestStepIndex, QuestStepState[] questStepStates)
    {
        info = _data;
        state = _state;
        _currentQuestStepIndex = currentQuestStepIndex;
        _questStepStates = questStepStates;
        if(questStepStates.Length != info.questStepPrefabs.Length)
        {
            Debug.Log("Data out of sign, reset data");
        }
    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool CurrentStepExists() => _currentQuestStepIndex < info.questStepPrefabs.Length;

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id, _currentQuestStepIndex, _questStepStates[_currentQuestStepIndex].state);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questPrefab = null;
        if (CurrentStepExists())
        {
            questPrefab = info.questStepPrefabs[_currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Out of range step prefab" + "No current step for Quest id" + info.id + ", stepindex " + _currentQuestStepIndex);
        }

        return questPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if (stepIndex < _questStepStates.Length)
        {
            _questStepStates[stepIndex].state = questStepState.state;
            _questStepStates[stepIndex].status = questStepState.status;
        }
        else
        {
            Debug.LogWarning("Index out of range: " + "Quest id" + info.id + ", Step Index: " + stepIndex);
        }
    }

    public QuestData GetQuestData() => new QuestData(state, _currentQuestStepIndex, _questStepStates);

    public string GetCurrentStateToText()
    {
        return _questStepStates[_currentQuestStepIndex].status;
    }


}
