using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO _quest;
    private string _questId;
    private Enums.QuestState _currentQuestState;

    public bool startPoint;
    public bool finishPoint;

    private QuestIcon _questIcon;

    private void Awake()
    {
        _questId = _quest.id;
        _questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void Start()
    {
        GameEventManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    public void GiveQuest()
    {
        if(_currentQuestState.Equals(Enums.QuestState.CanStart) && startPoint)
        {
            GameEventManager.Instance.questEvents.StartQuest(_questId);
        }
        else if(_currentQuestState.Equals(Enums.QuestState.CanFinish) && finishPoint)
        {
            GameEventManager.Instance.questEvents.FinishQuest(_questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if(quest.info.id.Equals(_questId))
        {
            _currentQuestState = quest.state;
            _questIcon.SetState(_currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnDisable()
    {
        GameEventManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

}
