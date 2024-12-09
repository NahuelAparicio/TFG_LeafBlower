using UnityEngine;

public class CustomGravityHandler : MonoBehaviour
{
    public float _gravity;
    public void ApplyAdditiveGravity(Rigidbody rb) => rb.AddForce(Vector3.up * _gravity, ForceMode.Acceleration);
}
