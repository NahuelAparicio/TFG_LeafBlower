using UnityEngine;

public class NPCController : MonoBehaviour
{

    public Animator animController;

    bool startRun = false;

    public GameObject[] objectsToFollow;

    public float speedToFollow;
    private int _currentTargetIndex = 0;
    private Transform _currentTarget;

    void Update()
    {
        if (!startRun || objectsToFollow.Length == 0) return;

        if (_currentTarget == null && _currentTargetIndex < objectsToFollow.Length)
        {
            _currentTarget = objectsToFollow[_currentTargetIndex].transform;
        }

        if (_currentTarget != null)
        {
            Vector3 direction = (_currentTarget.position - transform.position);
            direction.y = 0; 
            Vector3 moveDir = direction.normalized;

            transform.position += moveDir * speedToFollow * Time.deltaTime;

            // Orientarse hacia el punto
            if (moveDir != Vector3.zero)
                transform.forward = moveDir;

            if (direction.magnitude < 0.2f)
            {
                _currentTargetIndex++;
                _currentTarget = _currentTargetIndex < objectsToFollow.Length
                    ? objectsToFollow[_currentTargetIndex].transform
                    : null;

                if (_currentTarget == null)
                {
                    animController.SetBool("Run", false);
                    startRun = false;
                    Destroy(gameObject);
                }
            }
        }
    }

    public void StartRun()
    {
        animController.SetBool("Run", true);
        startRun = true;
        _currentTargetIndex = 0;
        _currentTarget = null;
    }

    //Shows a singles text message, if its to long it will be divided in differen "dialogue boxes"
    //public void ShowMessage(string message)
    //{
    //    if(message.Length > _maxCharsPerDialogue)
    //    {
    //        //Divide the string and add to the list 
    //        for (int i = 0; i < message.Length; i++)
    //        {

    //        }
    //    }
    //    _currentDialogue.Add(message);

    //    _dialogueHolder.SetActive(true);
    //    _dialogueText.text = message;
    //}
}
