using UnityEngine;

public class LeafBlower : MonoBehaviour
{
    private PlayerController _player;

    private void Awake()
    {
        _player = transform.parent.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_player.Inputs.IsBlowing() && !_player.Inputs.IsAspiring()) return;

        var movable = other.GetComponent<IMovable>();

        if(_player.Inputs.IsBlowing())
        {
            movable.OnBlow();
        }

        if(_player.Inputs.IsAspiring())
        {
            movable.OnAspire();
        }
    }
}
