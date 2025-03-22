using UnityEngine;

public class HandleBasket : MonoBehaviour
{
    public string idQuestAttached;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            GameEventManager.Instance.triggerEvents.TriggerBasket(idQuestAttached);
        }
    }
}
