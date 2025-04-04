using UnityEngine;

public class HandleFootballBall : HandleBallTrigger
{
    [SerializeField]private PositionToShoot _positionToShoot;

    protected override void HandleTriggerEnter()
    {
        if(_positionToShoot.IsInPosition)
        {
            base.HandleTriggerEnter();
        }
    }

}
