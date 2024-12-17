using UnityEngine;

public class Cheats : MonoBehaviour
{
    private PlayerController _player;
    private PlayerInputsActions _actions;

    public GameObject low, medium, heavy, superHeavy;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _actions = new PlayerInputsActions();
        _actions.Cheats.Enable();
        _actions.Cheats.LowWeight.performed += LowWeight_performed;
        _actions.Cheats.MediumWeight.performed += MediumWeight_performed;
        _actions.Cheats.HeavyWeight.performed += HeavyWeight_performed;
        _actions.Cheats.SuperHeavyWeight.performed += SuperHeavyWeight_performed;
    }

    private void SuperHeavyWeight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameObject go = Instantiate(superHeavy, GetSpawnPos(), Quaternion.identity);
    }

    private void HeavyWeight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameObject go = Instantiate(heavy, GetSpawnPos(), Quaternion.identity);
    }

    private void MediumWeight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameObject go = Instantiate(medium, GetSpawnPos(), Quaternion.identity);
    }

    private void LowWeight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameObject go = Instantiate(low, GetSpawnPos(), Quaternion.identity);
    }

    private Vector3 GetSpawnPos() => transform.position + _player.Movement.MoveDirection.normalized * 2;

    private void OnDestroy()
    {
        _actions.Cheats.LowWeight.performed -= LowWeight_performed;
        _actions.Cheats.MediumWeight.performed -= MediumWeight_performed;
        _actions.Cheats.HeavyWeight.performed -= HeavyWeight_performed;
        _actions.Cheats.SuperHeavyWeight.performed -= SuperHeavyWeight_performed;
    }
}
