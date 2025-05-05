using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputs _inputs;
    private PlayerMovement _movement;
    private PlayerHUD _hud;
    private PlayerInventory _inventory;
    private DialogueController _dialogueController;
    private PlayerStats _stats;
    private PlayerStamina _stamina;
    internal Camera _mainCamera;
    public bool isTalking;

    [SerializeField] private LeafBlower _leafBlower;
    [SerializeField] private PlayerInteractable _interactable;


    public LeafBlower LeafBlower => _leafBlower;
    public PlayerStats Stats => _stats;
    public PlayerHUD Hud => _hud;
    public PlayerInventory Inventory => _inventory;
    public PlayerInputs Inputs => _inputs;
    public PlayerInteractable Interactable => _interactable;
    public PlayerMovement Movement => _movement;
    public Camera MainCamera => _mainCamera;
    public PlayerStamina Stamina => _stamina;

    public bool jordanUnlocked = false;
    public bool hoverUnlocked = false;

    public Animator animator;

    private void Awake()
    {
        _stamina = GetComponent<PlayerStamina>();
        _stats = GetComponent<PlayerStats>();
        _hud = GetComponent<PlayerHUD>();
        _inventory = GetComponent<PlayerInventory>();
        _dialogueController = FindObjectOfType<DialogueController>();
        _inputs = GetComponent<PlayerInputs>();
        _movement = GetComponent<PlayerMovement>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _dialogueController.DialogueStarted += OnDialogueStarted;
        _dialogueController.DialogueEnded += OnDialogueEnded;
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused || isTalking) return;

        _movement.HandleAllMovement();
    }

    private void OnDialogueStarted()
    {
        isTalking = true;
    }
    private void OnDialogueEnded()
    {
        isTalking = false;
    }

    private void OnDestroy()
    {
        _dialogueController.DialogueStarted -= OnDialogueStarted;
        _dialogueController.DialogueEnded -= OnDialogueEnded;
    }

}
