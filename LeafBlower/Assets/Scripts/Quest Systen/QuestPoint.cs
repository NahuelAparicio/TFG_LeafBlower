using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestData _quest;
    private string _questId;
    private Enums.QuestState _currentQuestState;

    public bool startPoint;
    public bool finishPoint;

    private void Awake()
    {
        _questId = _quest.id;
    }

    private void OnEnable()
    {
        GameEventManager.Instance.questEvents.onQuestStateChange += QuestEvents_onQuestStateChange;
    }
    
    public void GiveQuest()
    {

        if(_currentQuestState.Equals(Enums.QuestState.Unlocked) && startPoint)
        {
            GameEventManager.Instance.questEvents.StartQuest(_questId);
        }
        else if(_currentQuestState.Equals(Enums.QuestState.Finished) && finishPoint)
        {
            GameEventManager.Instance.questEvents.FinishQuest(_questId);
        }
    }

    private void QuestEvents_onQuestStateChange(Quest quest)
    {
        if(quest.data.id.Equals(_questId))
        {
            _currentQuestState = quest.state;
        }
    }

    private void OnDisable()
    {
        GameEventManager.Instance.questEvents.onQuestStateChange -= QuestEvents_onQuestStateChange;
    }

}
