using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractingBool;
    public bool isInteractingStatus;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);    
    }

}
