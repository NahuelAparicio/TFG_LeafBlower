using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestData _quest;
    private string _questId;
    private Enums.QuestState _currentQuestState;

    public bool isStart;
    public bool isFinish;

    private void Awake()
    {
        _questId = _quest.id;
    }

    private void OnEnable()
    {
        GameEventManager.Instance.questEvents.onQuestStateChange += QuestEvents_onQuestStateChange;
    }
    

    private void QuestEvents_onQuestStateChange(Quest quest)
    {
        if(quest.data.id.Equals(_questId))
        {
            _currentQuestState = quest.State;
        }
    }

    private void OnDisable()
    {
        GameEventManager.Instance.questEvents.onQuestStateChange -= QuestEvents_onQuestStateChange;

    }

}
