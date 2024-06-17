using UnityEngine;
using Mirror;


public class Look : NetworkBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerCamera;
    [SerializeField] float sensX;
    [SerializeField] float sensY;

    private float multipl = 0.01f;
    private float xRotation;
    private float yRotation;


    public override void OnStartLocalPlayer()
    {
        playerCamera.gameObject.SetActive(true);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!isLocalPlayer) { return; }

        CameraInput();

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    
    void CameraInput()
    {
        yRotation += Input.GetAxisRaw("Mouse X") * sensX * multipl;
        xRotation -= Input.GetAxisRaw("Mouse Y") * sensY * multipl;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}