using Game.Player;
using UnityEngine;

/// <summary>
/// Держалка в дочках руки
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class GrabbableSimple : GrabbableBase
{
    protected override void OnStart()
    {
        base.OnStart();
        if (!offsetR)
        {
            offsetR = new GameObject("OffsetR").transform;
            offsetR.parent = transform;
        }

        if (!offsetL)
        {
            offsetL = new GameObject("OffsetL").transform;
            offsetL.parent = transform;
        }
    }

    public override Transform SetHand(TypeHand hand, bool active, Transform posHand)
    {
        if (active & (ActiveL | ActiveR))
        {
            return null;
        }
        
        Transform offset = InitHands(hand, active, posHand);

        if (ActiveR | ActiveL)
        {
            transform.parent = posHand;
            offset.position = posHand.position;
            offset.rotation = posHand.rotation;
        }
        else
        {
            transform.parent = null;
        }

        return offset;
    }
    
    protected override void TakeR() { }

    protected override void TakeOnlyR(){}

    protected override void TakeL() { }

    protected override void TakeOnlyL(){}

    protected override void TakeTwoHands(){}
}