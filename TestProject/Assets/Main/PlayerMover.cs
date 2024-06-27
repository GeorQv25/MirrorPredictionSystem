using HTC.UnityPlugin.VRModuleManagement;
using UnityEngine;


namespace VRInteraction.Example
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform playerMove;
        [SerializeField] private Transform playerRotate;

        private bool _enableSystem = false;
    
        void FixedUpdate()
        {
            IVRModuleDeviceState deviceStateR = VRModule.GetDeviceState(VRModule.GetRightControllerDeviceIndex());
            IVRModuleDeviceState deviceStateL = VRModule.GetDeviceState(VRModule.GetLeftControllerDeviceIndex());
            
            Move(deviceStateR);
            //Rotate(deviceStateL);
        }

        private void Move(IVRModuleDeviceState deviceState)
        {
            if (!_enableSystem)
                return;

            Vector3 forward = Camera.main.transform.forward * deviceState.GetAxisValue(VRModuleRawAxis.Axis0Y);
            forward = new Vector3(forward.x, 0, forward.z);
            Vector3 right = Camera.main.transform.right * deviceState.GetAxisValue(VRModuleRawAxis.Axis0X);
            right = new Vector3(right.x, 0, right.z);
            playerMove.position += (forward + right).normalized * speed * Time.fixedDeltaTime;
        }

        private void Rotate(IVRModuleDeviceState deviceState)
        {
            playerRotate.Rotate(deviceState.GetAxisValue(VRModuleRawAxis.Axis0X) * new Vector3(0, 1, 0));
        }

        public void SwithWorkType()
        { 
            _enableSystem = !_enableSystem;

            if (!_enableSystem)
                playerMove.position = Vector3.zero;
        }
    }

}