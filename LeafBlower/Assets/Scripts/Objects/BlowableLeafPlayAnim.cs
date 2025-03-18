using UnityEngine;

public class BlowableLeafPlayAnim : MonoBehaviour
{
    public Animation animationComponent;
    public AnimationClip animationClip;

    public void PlayLeafAnim()
    {
        if (animationComponent == null)
            animationComponent = GetComponent<Animation>();

        if (animationClip != null)
        {
            animationComponent.AddClip(animationClip, animationClip.name);
            animationComponent.Play(animationClip.name);
        }
    }
}
