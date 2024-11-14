using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerStats _stats;
    private PlayerInputs _inputs;
    private DialogueController _dialogueController;
    [SerializeField] private PlayerAnimations _animations;
    [SerializeField] private PlayerInteractable _interactable;
    [SerializeField] private CheckCollisions _checkCollisions;
    [SerializeField] private Enums.CharacterState _currentCharacterState = Enums.CharacterState.Idle;
    private Rigidbody _rb;

    public Enums.CharacterState CurrentCharacterState { get => _currentCharacterState; set => _currentCharacterState = value; }
    public PlayerMovement Movement => _movement;
    public PlayerInputs Inputs => _inputs;
    public PlayerStats Stats => _stats;
    public PlayerAnimations Animations => _animations;
    public PlayerInteractable Interactable => _interactable;
    public CheckCollisions CheckCollisions => _checkCollisions;
    public Rigidbody Rigidbody => _rb;

    public bool isInteracting, isStuned;

    public CapsuleCollider playerCollider;

    private bool _isTalking;
    public bool IsTalking => _isTalking;

    private void Awake()
    {
        isStuned = false;
        _movement = GetComponent<PlayerMovement>();
        _stats = GetComponent<PlayerStats>();
        _inputs = GetComponent<PlayerInputs>();
        _rb = GetComponent<Rigidbody>();
        _dialogueController = FindObjectOfType<DialogueController>();
        _dialogueController.DialogueStarted += OnDialogueStarted;
        _dialogueController.DialogueEnded += OnDialogueEnded;
    }

    void Update()
    {
        if (CurrentCharacterState == Enums.CharacterState.Idle)
        {
            _animations.HandleIdleAnimations();
        }
        else if (CurrentCharacterState == Enums.CharacterState.Moving)
        {
            _animations.HandleMovingAnimations();
        }
    }
    private void FixedUpdate()
    {
        if(!_isTalking)
            _movement.HandleAllMovement();
    }

    private void LateUpdate()
    {
        if(!GameManager.Instance.IsPaused)
        {
            isInteracting = _animations.Animator.GetBool("isInteracting");
            _movement.isJumping = _animations.Animator.GetBool("isJumping");
            _animations.Animator.SetBool("isGrounded", _checkCollisions.IsGrounded);
        }
    }

    public void ChangeCharacterState(Enums.CharacterState newState)
    {
        if(newState == _currentCharacterState)
        {
            return;
        }
        _currentCharacterState = newState;
    }

    private void OnDialogueStarted() => _isTalking = true;
    private void OnDialogueEnded() => _isTalking = false;

    private void OnDestroy()
    {
        _dialogueController.DialogueStarted -= OnDialogueStarted;
        _dialogueController.DialogueEnded -= OnDialogueEnded;
    }
}
