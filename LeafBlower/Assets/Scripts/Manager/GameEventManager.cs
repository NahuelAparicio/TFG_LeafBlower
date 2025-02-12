using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }
   
    public event Action<int> onLevelUp;

    public QuestEvents questEvents;
    public CollectingEvents collectingEvents;
    public TriggerEvents triggerEvents;
    public CameraEvents cameraEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }

        Instance = this;

        questEvents = new QuestEvents();
        collectingEvents = new CollectingEvents();
        triggerEvents = new TriggerEvents();
        cameraEvents = new CameraEvents();
    }

    public void PlayerLevelChange(int level) => onLevelUp?.Invoke(level);

}
