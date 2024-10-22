using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerStats _stats;
    private PlayerInputs _inputs;
    [SerializeField] private PlayerAnimations _animations;
    [SerializeField] private PlayerInteractable _interactable;
    [SerializeField] private CheckGround _checkGround;
    [SerializeField] private Enums.CharacterState _currentCharacterState = Enums.CharacterState.Idle;
    private Rigidbody _rb;

    public Enums.CharacterState CurrentCharacterState { get => _currentCharacterState; set => _currentCharacterState = value; }
    public PlayerMovement Movement => _movement;
    public PlayerInputs Inputs => _inputs;
    public PlayerStats Stats => _stats;
    public PlayerAnimations Animations => _animations;
    public PlayerInteractable Interactable => _interactable;
    public CheckGround CheckGround => _checkGround;
    public Rigidbody Rigidbody => _rb;

    public bool isInteracting, isStuned;

    private void Awake()
    {
        isStuned = false;
        _movement = GetComponent<PlayerMovement>();
        _stats = GetComponent<PlayerStats>();
        _inputs = GetComponent<PlayerInputs>();
        _rb = GetComponent<Rigidbody>();
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
        if(!isStuned)
            _movement.HandleAllMovement();
    }

    private void LateUpdate()
    {
        if(!GameManager.Instance.IsPaused)
        {
            isInteracting = _animations.Animator.GetBool("isInteracting");
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
}
