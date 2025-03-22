using UnityEngine;

public class NPCController : MonoBehaviour
{
    private NPCMovement _movement;
    [SerializeField] private NPCDialogue _dialogue;

    public NPCMovement Movement => _movement;
    public NPCDialogue Dialogue => _dialogue;

    private void Awake()
    {
        _movement = GetComponent<NPCMovement>();
    }

    void Update()
    {
        
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
