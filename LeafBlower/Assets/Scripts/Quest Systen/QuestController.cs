using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    private int _currentPlayerLevel = 1;
    public Dictionary<string, Quest> questMap = new Dictionary<string, Quest>();

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        GameEventManager.Instance.questEvents.onStartQuest += StartQuest;
        GameEventManager.Instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventManager.Instance.questEvents.onFinishQuest += FinishQuest;

        GameEventManager.Instance.onLevelUp += PlayerLevelChange;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventManager.Instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventManager.Instance.onLevelUp -= PlayerLevelChange;

    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            GameEventManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if(quest.state == Enums.QuestState.Locked && CheckRequirements(quest))
            {
                ChangeQuestState(quest.data.id, Enums.QuestState.Unlocked);
            }
        }
    }

    private void PlayerLevelChange(int level)
    {
        _currentPlayerLevel = level;
    }

    private bool CheckRequirements(Quest quest)
    {
        bool meetsRequirement = true;

        if(_currentPlayerLevel < quest.data.levelRequired)
        {
            meetsRequirement = false;
        }

        foreach (QuestData prerequisitsData in quest.data.questPreequisits)
        {
            if(GetQuestById(prerequisitsData.id).state != Enums.QuestState.Completed)
            {
                meetsRequirement = false;
            }
        }
        return meetsRequirement;
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(transform);
        ChangeQuestState(quest.data.id, Enums.QuestState.InProgress);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();

        if(quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(transform);
        }
        else
        {
            ChangeQuestState(quest.data.id, Enums.QuestState.Completed);
        }

    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.data.id, Enums.QuestState.Finished);
    }

    private void ClaimRewards(Quest quest)
    {
        //TODOO ----------- Gain Gold
        Debug.Log("You gained gold");
    }

    private void ChangeQuestState(string id, Enums.QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventManager.Instance.questEvents.QuestStateChange(quest);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestData[] allQuests = Resources.LoadAll<QuestData>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestData q in allQuests)
        {
            if(idToQuestMap.ContainsKey(q.id))
            {
                Debug.LogWarning("Duplicate ID found" + q.id);
            }
            idToQuestMap.Add(q.id, new Quest(q));
        }

        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        return quest;
    }
}
