using UnityEngine;
using Mirror;


public enum DragType
{
    AirDrag = 1,
    NormalDrag = 6,
}

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private LayerMask maskToAvoid;
    [SerializeField] private BallLauncher _launcher;

    private Rigidbody playerRb;
    private float height = 2f;
    private float heightOffset = 0.35f;
    private Vector3 moveDir;
    private float moveSepeed = 10f;
    private float speedMultipl = 6.5f;
    private float speedMultiplInAir = 0.6f;
    private float jumpForce = 8;
    private bool isGrounded;


    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isLocalPlayer) { return; }

        SetGroundState();
        SetDrag();
        PlayerInput();
    }

    void PlayerInput()
    {
        moveDir = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        else if (Input.GetMouseButtonDown(0) && _launcher)
        {
            _launcher.Shoot();
        }
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
            playerRb.AddForce(moveDir.normalized * moveSepeed * speedMultipl, ForceMode.Acceleration);
            return;
        }
        playerRb.AddForce(moveDir.normalized * moveSepeed * speedMultiplInAir, ForceMode.Acceleration);
    }

    private void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}