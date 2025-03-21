using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    private FirstPersonInputs _inputs;
    [SerializeField] private PlayerInteractable _interactable;
    private FirstPersonMovement _movement;
    internal Camera _mainCamera;

    public bool isTalking;

    public FirstPersonInputs Inputs => _inputs;
    public PlayerInteractable Interactable => _interactable;
    public FirstPersonMovement Movement => _movement;
    public Camera MainCamera => _mainCamera;

    private void Awake()
    {
        _inputs = GetComponent<FirstPersonInputs>();
        _movement = GetComponent<FirstPersonMovement>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        _movement.HandleAllMovement();
    }


}
