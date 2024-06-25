using System.Collections;
using Game.Player;
using HTC.UnityPlugin.VRModuleManagement;
using Mirror;
using UnityEngine;


public class HandGrabber : MonoBehaviour
{
    [SerializeField] private TypeHand typeHand;
    [SerializeField] private HandCollision collision;
    [SerializeField] private Transform handPos;
    [SerializeField] private Transform offsetIK;
    [SerializeField] private float distanceGrab;
    
    private GrabbableBase _grabObject;
    private Transform _takenBall;
    private bool _taken;
    private bool _distanceBreak;
    private Vector3 _offsetPosSave;
    private Quaternion _offsetQuartSave;
    

    [Client]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GrabbableBase ballOut) & !_grabObject)
        {
            _grabObject = ballOut;
        }
    }

    [Client]
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GrabbableBase ballOut) & !_taken)
        {
            if (_grabObject == ballOut)
            {
                _grabObject = null;
            }
        }
    }

    [Client]
    private void FixedUpdate()
    {
        IVRModuleDeviceState deviceState = VRModule.GetDeviceState( 
            typeHand == TypeHand.Right ?
            VRModule.GetRightControllerDeviceIndex() : VRModule.GetLeftControllerDeviceIndex());
        
        if (_taken)
        {
            offsetIK.position =  Vector3.Lerp(offsetIK.position,  _takenBall.position, 0.9f);
            offsetIK.rotation = Quaternion.Lerp(offsetIK.rotation, _takenBall.rotation, 0.9f);
            
            _distanceBreak = Mathf.Abs(Vector3.SqrMagnitude(handPos.position - _takenBall.position)) > distanceGrab;
            
            if (!deviceState.GetButtonPress(VRModuleRawButton.Grip) & _grabObject || _distanceBreak)
            {
                Throw();

                if (_distanceBreak)
                {
                    _grabObject = null;
                }
            }
        }
        else
        {
            if (deviceState.GetButtonPress(VRModuleRawButton.Grip) & _grabObject)
            {  
                Take();
            }
        }
    }

    private void Take()
    {
        _takenBall = _grabObject.SetHand(typeHand, true, handPos);
        if (!_takenBall)
        {
            return;
        }
        _grabObject.gameObject.layer = LayerMask.NameToLayer("HandedBall");
        _taken = true;
        SetHandActive(false);
        collision.TogglePhysics(false);
    }

    private void Throw()
    {
        _grabObject.SetHand(typeHand, false, handPos);
        SetHandActive(true);
        StartCoroutine(TurnOnPhysics(_grabObject.gameObject));
        _grabObject = null;
        _takenBall = null;
        _taken = false;
    }

    private void SetHandActive(bool active)
    {
        if (active)
        {
            offsetIK.localPosition = _offsetPosSave;
            offsetIK.localRotation = _offsetQuartSave;
        }
        else
        {
            _offsetPosSave = offsetIK.localPosition;
            _offsetQuartSave = offsetIK.localRotation;
        }
    }

    private IEnumerator TurnOnPhysics(GameObject grabObject)
    {
        yield return new WaitForSeconds(1.5f);
        grabObject.gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(1.5f);
        if (!_taken)
        {
            collision.TogglePhysics(true);
        }
    }
}
