using UnityEngine;

public class HandleBallTrigger : MonoBehaviour
{
    public string idQuestAttached;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            if(other.GetComponent<NormalObject>().HasBeenShoot)
            {
                HandleTriggerEnter();
            }
        }
    }

    protected virtual void HandleTriggerEnter()
    {
        GameEventManager.Instance.triggerEvents.InvokeBallTriggered(idQuestAttached);
    }
}
