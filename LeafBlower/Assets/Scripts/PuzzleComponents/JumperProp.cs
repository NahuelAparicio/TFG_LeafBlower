using UnityEngine;

public class JumperProp : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().AddExternalJumpForce(jumpForce);
        }
    }
}
