using Game.Player;
using UnityEngine;

//Держалка мяча
public class BallGrabbable : GrabbableBase
{
    [SerializeField] protected Transform handRotatorR;
    [SerializeField] protected Transform handRotatorL;
    
    [SerializeField] protected Transform wristRotatorR;
    [SerializeField] protected Transform wristRotatorL;

    protected override void OnStart()
    {
        base.OnStart();
        _localPosR.parent = handRotatorR;
        _localPosL.parent = handRotatorL;
    }

    public override Transform SetHand(TypeHand hand, bool active, Transform posHand)
    {
        return InitHands(hand, active, posHand);
    }

    protected override void TakeR()
    {
        handRotatorR.LookAt(_posHandR);
        RotateWrist(_posHandR, _localPosR, wristRotatorR);
    }

    protected override void TakeOnlyR()
    {
        transform.position = _posHandR.position - _posHandR.right / 10;
        transform.rotation = _posHandR.rotation * Quaternion.Inverse(_startRotHand) * _startRot;
    }

    protected override void TakeL()
    {
        handRotatorL.LookAt(_posHandL);
        RotateWrist(_posHandL, _localPosL, wristRotatorL);
    }

    protected override void TakeOnlyL()
    {
        transform.rotation = _posHandL.rotation * Quaternion.Inverse(_startRotHand) * _startRot;
        transform.position = _posHandL.position + _posHandL.right / 10;
    }

    protected override void TakeTwoHands()
    {
        transform.position = _posHandL.position - (_posHandL.position - _posHandR.position) / 2;
    }
}
