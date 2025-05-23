using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestController : MonoBehaviour
{
    [SerializeField] private bool _loadQuestState = true;

    private Dictionary<string, Quest> _questMap = new Dictionary<string, Quest>();

    public GameObject questText;
    public RectTransform questTextTarget;

 
    private void OnDisable()
    {
        GameEventManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventManager.Instance.questEvents.onFinishQuest -= FinishQuest;
        GameEventManager.Instance.questEvents.onQuestStepChange -= QuestStepStateChange;


        foreach (Quest quest in _questMap.Values)
        {
            SaveQuest(quest);
        }
    }

    private void Start()
    {
        _questMap = CreateQuestMap();
        GameEventManager.Instance.questEvents.onStartQuest += StartQuest;
        GameEventManager.Instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventManager.Instance.questEvents.onFinishQuest += FinishQuest;
        GameEventManager.Instance.questEvents.onQuestStepChange += QuestStepStateChange;

        foreach (Quest quest in _questMap.Values)
        {
            if(quest.state == Enums.QuestState.InProgress)
            {
                quest.text = InstantiateQuestText(quest);
                quest.InstantiateCurrentQuestStep(transform);
            }
            GameEventManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    private void Update()
    {
        foreach (Quest quest in _questMap.Values)
        {
            if(quest.state == Enums.QuestState.RequirementNotMet && CheckRequirements(quest))
            {
                ChangeQuestState(quest.info.id, Enums.QuestState.CanStart);
            }
        }
    }


    private bool CheckRequirements(Quest quest)
    {
        bool meetsRequirement = true;

        foreach (QuestInfoSO prerequisitsData in quest.info.questPreequisits)
        {
            if(GetQuestById(prerequisitsData.id).state != Enums.QuestState.Finished)
            {
                meetsRequirement = false;
                break;
            }
        }
        return meetsRequirement;
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(transform);
        quest.text = InstantiateQuestText(quest);
        ChangeQuestState(quest.info.id, Enums.QuestState.InProgress);
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
            ChangeQuestState(quest.info.id, Enums.QuestState.CanFinish);
        }

    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        Destroy(quest.text.gameObject);
        ChangeQuestState(quest.info.id, Enums.QuestState.Finished);
    }
    private TextMeshProUGUI InstantiateQuestText(Quest quest)
    {
        GameObject textUI = Instantiate(questText, questTextTarget);
        RectTransform rectTransform = textUI.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;  
        rectTransform.localScale = Vector3.one;

        TextMeshProUGUI text = textUI.GetComponent<TextMeshProUGUI>();
        text.text = quest.info.description + " " + quest.GetCurrentStateToText();

        return text;
    }
    private void ClaimRewards(Quest quest)
    {
        GameEventManager.Instance.collectingEvents.InvokeCollectCollectionable(Enums.CollectionableType.Coin, quest.info.goldReward);
    }

    private void ChangeQuestState(string id, Enums.QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventManager.Instance.questEvents.QuestStateChange(quest);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO q in allQuests)
        {
            if(idToQuestMap.ContainsKey(q.id))
            {
                Debug.LogWarning("Duplicate ID found" + q.id);
            }
            idToQuestMap.Add(q.id, LoadQuest(q));
        }

        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = _questMap[id];
        return quest;
    }
    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
        quest.text.text = quest.info.description + " " + quest.GetCurrentStateToText();
    }

    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questSaveData = quest.GetQuestData();
            string serializedData = JsonUtility.ToJson(questSaveData);
            PlayerPrefs.SetString(quest.info.id, serializedData);
        }
        catch(System.Exception e)
        {
            Debug.LogError("Failed to save quest with id " + quest.info.id + " : " + e);
        }
    }

    private Quest LoadQuest(QuestInfoSO questInfo)
    {
        Quest quest = null;
        try
        {
            if(PlayerPrefs.HasKey(questInfo.id) && _loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id); 
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
                GameEventManager.Instance.questEvents.QuestStateChange(quest);
            }
            else
            {
                quest = new Quest(questInfo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load quest with" + e);
        }
        return quest;
    }


}
