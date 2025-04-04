using UnityEngine;

public class PositionToShoot : MonoBehaviour
{
    private bool _isInPosition;
    public bool IsInPosition => _isInPosition;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _isInPosition = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isInPosition = false;
        }
    }
}
