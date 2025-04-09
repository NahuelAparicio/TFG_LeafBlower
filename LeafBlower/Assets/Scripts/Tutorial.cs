using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    public UnityEvent onTriggerEnter;

    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        onTriggerExit.Invoke();
    }


}
