using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    protected bool isFinished = false;
    protected string _questId;
    private int _stepIndex;

    public void InitializeQuestStep(string id, int stepIndex, string questStepState)
    {
        _questId = id;
        _stepIndex = stepIndex;
        if(questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }
    }
    protected void FinishQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;

            GameEventManager.Instance.questEvents.AdvanceQuest(_questId);

            Destroy(gameObject);
        }
    }

    protected void ChangeState(string newState, string newStatus)
    {
        GameEventManager.Instance.questEvents.QuestStepStateChange(_questId, _stepIndex, new QuestStepState(newState, newStatus));
    }

    protected abstract void SetQuestStepState(string state);
}
