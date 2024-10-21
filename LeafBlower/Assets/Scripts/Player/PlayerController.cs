using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerStats _stats;
    private PlayerInputs _inputs;
    [SerializeField] private PlayerInteractable _interactable;
    [SerializeField] private CheckGround _checkGround;
    private Rigidbody _rb;

    public PlayerMovement Movement => _movement;
    public PlayerInputs Inputs => _inputs;
    public PlayerStats Stats => _stats;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(!isStuned)
            _movement.HandleAllMovement();
    }
}
