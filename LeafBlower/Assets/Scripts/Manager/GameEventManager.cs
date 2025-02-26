using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }
   
    public QuestEvents questEvents;
    public CollectingEvents collectingEvents;
    public TriggerEvents triggerEvents;
    public CameraEvents cameraEvents;
    public PlayerEvents playerEvents;

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
        playerEvents = new PlayerEvents();
    }


}
