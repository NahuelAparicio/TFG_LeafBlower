using UnityEngine;

public class PlayerRespawnHandler : MonoBehaviour
{
    private PlayerController _player;
    private float _currentTime = 0f;
    [SerializeField] private float _timeToSavePos = 2f;

    [SerializeField] private Vector3 positionToRespawn;

    private void Awake()
    {
        _player = transform.parent.GetComponent<PlayerController>();
        positionToRespawn = _player.transform.position;
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        if(_currentTime >= _timeToSavePos)
        {
            if(_player.CheckCollisions.IsGrounded)
            {
                positionToRespawn = _player.transform.position;
            }
            else
            {
                _currentTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Respawner"))
        {
            OnRespawn();
        }
    }

    private void OnRespawn()
    {
        _player.Rigidbody.velocity = Vector3.zero;
        _player.Rigidbody.angularVelocity = Vector3.zero;
        _player.transform.position = positionToRespawn;
    }
}
