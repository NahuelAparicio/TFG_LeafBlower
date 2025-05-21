using FMODUnity;
using UnityEngine;
using FMODUnity;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO _quest;
    private string _questId;
    private Enums.QuestState _currentQuestState;

    public bool startPoint;
    public bool finishPoint;

    private QuestIcon _questIcon;

    public NPCDialogue dialogue;
    public bool hasEndDialogue = false;
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
            dialogue.EnableDialogueAdd();
            GameEventManager.Instance.questEvents.FinishQuest(_questId);
        }
    }

    public void EndQuest()
    {
        if (_currentQuestState.Equals(Enums.QuestState.CanFinish) && finishPoint)
        {
            dialogue.EnableDialogueAdd();
            dialogue.AddNewDialogue();
            GameEventManager.Instance.questEvents.FinishQuest(_questId);

            // Reproduce el sonido de recompensa de misión
            RuntimeManager.PlayOneShot("event:/UI/Mision_Reward");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!finishPoint) return;
        if (hasEndDialogue) return;
        if (other.CompareTag("Player"))
        {
            if (_currentQuestState.Equals(Enums.QuestState.CanFinish) && finishPoint)
            {
                hasEndDialogue = true;
                dialogue.EnableDialogueAdd();
                dialogue.AddNewDialogue();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!finishPoint) return;
        if (hasEndDialogue) return;
        if (other.CompareTag("Player"))
        {
            if (_currentQuestState.Equals(Enums.QuestState.CanFinish) && finishPoint)
            {
                hasEndDialogue = true;
                dialogue.EnableDialogueAdd();
                dialogue.AddNewDialogue();
            }
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
