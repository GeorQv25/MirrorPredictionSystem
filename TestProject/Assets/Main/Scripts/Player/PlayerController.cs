using UnityEngine;
using Mirror;
using HTC.UnityPlugin.VRModuleManagement;


public enum DragType
{
    AirDrag = 1,
    NormalDrag = 6,
}

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private bool _wasPressed;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LayerMask maskToAvoid;
    [SerializeField] private BallLauncher _launcher;
    [SerializeField] private bool _isTestMode;

    private ControllerBase _controller;
    private Rigidbody playerRb;
    private float height = 2f;
    private float heightOffset = 0.35f;
    private bool isGrounded;


    public void Initialize(ControllerBase controller)
    {
        _controller = controller;
    }

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isLocalPlayer) { return; }
        _controller.SetDirection();

        SetGroundState();
        SetDrag();
        BallPush();
    }


    void SetGroundState()
    {
        Collider[] result = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y - height / 2 + heightOffset, transform.position.z), 0.45f, ~maskToAvoid);
        isGrounded = result.Length > 0;
    }

    void SetDrag()
    {
        playerRb.drag = isGrounded ? (float) DragType.NormalDrag : (float)DragType.AirDrag;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) { return; }

        if (isGrounded)
        {
            _controller.Move();
        }
    }



    private void VRMovement()
    {
        //IVRModuleDeviceState deviceState = VRModule.GetDeviceState(VRModule.GetRightControllerDeviceIndex());
        //IVRModuleDeviceState deviceStateL = VRModule.GetDeviceState(VRModule.GetLeftControllerDeviceIndex());

        //Vector3 forward = Camera.main.transform.forward * deviceState.GetAxisValue(VRModuleRawAxis.Axis0Y);
        //forward = new Vector3(forward.x, 0, forward.z);
        //Vector3 right = Camera.main.transform.right * deviceState.GetAxisValue(VRModuleRawAxis.Axis0X);
        //right = new Vector3(right.x, 0, right.z);
        //moveDir = (forward + right).normalized;

        ////Debug.Log(deviceStateL.GetAxisValue(VRModuleRawAxis.Axis0X));
        //_cameraHolder.Rotate(deviceStateL.GetAxisValue(VRModuleRawAxis.Axis0X) * new Vector3(0, 1, 0));
        ////playerMove.position += (forward + right).normalized * speed * Time.fixedDeltaTime;
    }

    private void BallPush()
    {
        IVRModuleDeviceState deviceState = VRModule.GetDeviceState(VRModule.GetRightControllerDeviceIndex());
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, _rightHand.position);
        _lineRenderer.SetPosition(1, _rightHand.position + _rightHand.forward * 10);
        if (!deviceState.GetButtonPress(VRModuleRawButton.A))
        {
            //_lineRenderer.enabled = false;
            _wasPressed = false;
            return;
        }
        if (!_wasPressed)
        {
            ApplyForceToBall();
        }
        _wasPressed = true;
    }

    private void ApplyForceToBall()
    {
        if(Physics.Raycast(_rightHand.position, _rightHand.forward, out RaycastHit hitInfo, 10))
        {
            Debug.Log(hitInfo.transform.name);
            if(hitInfo.transform.TryGetComponent(out Rigidbody rb))
            {
                hitInfo.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                hitInfo.rigidbody.velocity = Vector3.zero;
                hitInfo.rigidbody.constraints = RigidbodyConstraints.None;
                hitInfo.transform.position = _rightHand.position;
                //rb.AddForce(-(hitInfo.transform.position - transform.position).normalized * 14 + Vector3.up * 3, ForceMode.Impulse);
            }
        }
    }
}

//void PlayerInput()
//{

//    // TEST
//    if (isServer && _isTestMode)
//    {
//        if (transform.position.x > 5) moveDir = new Vector3(-1, 0, 0);
//        else if (transform.position.x < -5) moveDir = new Vector3(1, 0, 0);
//    }
//}

//private void KeyBoardMovement()
//{
//    if (Input.GetMouseButtonDown(0) && _launcher)
//    {
//        _launcher.Shoot();
//    }
//}