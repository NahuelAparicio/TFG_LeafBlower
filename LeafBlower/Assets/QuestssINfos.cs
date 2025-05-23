using UnityEngine;

public class QuestssINfos : MonoBehaviour
{
    [Header("Quest")]

    [SerializeField] private QuestInfoSO _quest;
    private string _questId;
    private Enums.QuestState _currentQuestState;

    public bool startPoint;
    public bool finishPoint;
    private void Awake()
    {
        _questId = _quest.id;
    }

    private void Start()
    {
        GameEventManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }
    public void GiveQuest()
    {
        if (_currentQuestState.Equals(Enums.QuestState.CanStart) && startPoint)
        {
            GameEventManager.Instance.questEvents.StartQuest(_questId);
        }
        else if (_currentQuestState.Equals(Enums.QuestState.CanFinish) && finishPoint)
        {
            GameEventManager.Instance.questEvents.FinishQuest(_questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(_questId))
        {
            _currentQuestState = quest.state;
        }
    }
}
