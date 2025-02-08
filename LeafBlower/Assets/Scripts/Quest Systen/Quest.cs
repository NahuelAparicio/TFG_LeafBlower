using UnityEngine;
using static Enums;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;

    private int _currentQuestStepIndex;
    private QuestStepState[] _questStepStates;

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

    public string GetFullStatusText()
    {
        string fullStatus = "";

        if (state == QuestState.RequirementNotMet)
        {
            fullStatus = "Requirements are not yet met to start this quest.";
        }
        else if (state == QuestState.CanStart)
        {
            fullStatus = "This quest can be started!";
        }
        else
        {
            // display all previous quests with strikethroughs
            for (int i = 0; i < _currentQuestStepIndex; i++)
            {
                fullStatus += "<s>" + _questStepStates[i].status + "</s>\n";
            }
            // display the current step, if it exists
            if (CurrentStepExists())
            {
                fullStatus += _questStepStates[_currentQuestStepIndex].status;
            }
            // when the quest is completed or turned in
            if (state == QuestState.CanFinish)
            {
                fullStatus += "The quest is ready to be turned in.";
            }
            else if (state == QuestState.Finished)
            {
                fullStatus += "The quest has been completed!";
            }
        }

        return fullStatus;
    }
}
