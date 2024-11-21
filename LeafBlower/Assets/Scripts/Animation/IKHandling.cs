using UnityEngine;

public class IKHandling : MonoBehaviour
{
    [SerializeField]private float _offsetY;
    private Animator _anim;
    //public Transform leftIKTarget, rightIKTarget;
    //public float ikWeight = 1f;
    //public Transform hintLeft, hintRight;

    private Vector3 _rightFootPosition, _leftFootPosition;

    private Quaternion _rightFootRotation, _leftFootRotation;

    private float _rightFootWeight, _leftFootWeight;

    private Transform _rightFoot, _leftFoot;


    private void Awake()
    {
        _anim = GetComponent<Animator>();

        _rightFoot = _anim.GetBoneTransform(HumanBodyBones.RightFoot);
        _leftFoot = _anim.GetBoneTransform(HumanBodyBones.LeftFoot);

        _rightFootRotation = _rightFoot.rotation;
        _leftFootRotation = _leftFoot.rotation;
    }

    private void Update()
    {
        RaycastHit rigthHit, leftHit;

        Vector3 rPos = _rightFoot.position;
        Vector3 lPos = _leftFoot.position;

        if(Physics.Raycast(rPos, Vector3.down, out rigthHit, 1))
        {
            _rightFootPosition = rigthHit.point + new Vector3(0, _offsetY, 0);
            _rightFootRotation = Quaternion.FromToRotation(transform.up, rigthHit.normal) * transform.rotation;
        }
        if (Physics.Raycast(lPos, Vector3.down, out leftHit, 1))
        {
            _leftFootPosition = leftHit.point + new Vector3(0, _offsetY, 0);
            _leftFootRotation = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _rightFootWeight = _anim.GetFloat(Constants.ANIM_RIGHT_FOOT);
        _leftFootWeight = _anim.GetFloat(Constants.ANIM_LEFT_FOOT);

        _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, _rightFootWeight);
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _leftFootWeight);

        _anim.SetIKPosition(AvatarIKGoal.RightFoot, _rightFootPosition);
        _anim.SetIKPosition(AvatarIKGoal.LeftFoot, _leftFootPosition);

        _anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, _rightFootWeight);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _leftFootWeight);

        _anim.SetIKRotation(AvatarIKGoal.RightFoot, _rightFootRotation);
        _anim.SetIKRotation(AvatarIKGoal.LeftFoot, _leftFootRotation);

        //_anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, ikWeight);
        //_anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, ikWeight);

        //_anim.SetIKHintPosition(AvatarIKHint.RightKnee, hintRight.position);
        //_anim.SetIKHintPosition(AvatarIKHint.LeftKnee, hintLeft.position);
    }

}
