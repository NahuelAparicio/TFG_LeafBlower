using UnityEngine;

public class CustomGravityHandler : MonoBehaviour
{
    private float _gravity;
    [SerializeField] private float _normalGravity = -35;
    [SerializeField] private float _fallingGravity = -60f;
    public void SetFallingGravity()
    {
        if (_gravity == _fallingGravity) return;
        _gravity = _fallingGravity;
    }

    public void SetNormalGravity()
    {
        if (_gravity == _normalGravity) return;
        _gravity = _normalGravity;
    }
    public void ApplyAdditiveGravity(Rigidbody rb) => rb.AddForce(Vector3.up * _gravity, ForceMode.Acceleration);
}
