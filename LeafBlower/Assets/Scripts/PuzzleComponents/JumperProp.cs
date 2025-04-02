using UnityEngine;

public class JumperProp : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().AddExternalForce(GetDirection(other.transform.position) * jumpForce);
        }
    }

    private Vector3 GetDirection(Vector3 pos) => Vector3.up;
}
