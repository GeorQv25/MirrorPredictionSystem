using HTC.UnityPlugin.VRModuleManagement;
using UnityEngine;


public class VRController : ControllerBase
{
    public VRController(Transform relativeTransform, Rigidbody rigidbody, float speed) : base(relativeTransform, rigidbody, speed)
    {

    }

    public void Rotate()
    {
    
    }

    public override void SetDirection()
    {
        IVRModuleDeviceState deviceState = VRModule.GetDeviceState(VRModule.GetRightControllerDeviceIndex());
        //IVRModuleDeviceState deviceStateL = VRModule.GetDeviceState(VRModule.GetLeftControllerDeviceIndex());

        Vector3 forward = Camera.main.transform.forward * deviceState.GetAxisValue(VRModuleRawAxis.Axis0Y);
        forward = new Vector3(forward.x, 0, forward.z);
        Vector3 right = Camera.main.transform.right * deviceState.GetAxisValue(VRModuleRawAxis.Axis0X);
        right = new Vector3(right.x, 0, right.z);

        _moveDir = (forward + right).normalized;
    }
}