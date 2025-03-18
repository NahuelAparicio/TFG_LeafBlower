using UnityEngine;

public class Cheats : MonoBehaviour
{
    private PlayerController _player;
    private PlayerInputsActions _actions;

    public GameObject low, medium, heavy, superHeavy;
    public bool activeCheats = false;

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.M))
        {
            activeCheats = true;
        }
    }

    private void SuperHeavyWeight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(activeCheats)
        {
            GameObject go = Instantiate(superHeavy, GetSpawnPos(), Quaternion.identity);
        }
    }

    private void HeavyWeight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (activeCheats)
        {
            GameObject go = Instantiate(heavy, GetSpawnPos(), Quaternion.identity);

        }
    }

    private void MediumWeight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(activeCheats)
        {
            GameObject go = Instantiate(medium, GetSpawnPos(), Quaternion.identity);
        }
    }

    private void LowWeight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (activeCheats)
        {
            GameObject go = Instantiate(low, GetSpawnPos(), Quaternion.identity);
        }
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
