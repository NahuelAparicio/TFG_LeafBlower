using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractingBool, isJumpingBool;
    public bool isInteractingStatus, isJumpingStatus;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);  
        animator.SetBool(isJumpingBool, isJumpingStatus);
    }

}
