using Mirror;
using UnityEngine;


public class CameraChange : NetworkBehaviour
{
    [SerializeField] private GameObject[] _cameras;
    private GameObject _lastActiveCamera;


    private void SetCamera(int cameraNumber)
    {
        if (cameraNumber > _cameras.Length - 1 || _lastActiveCamera == _cameras[cameraNumber])
        {
            return;
        }

        if (_lastActiveCamera)
        {
            _lastActiveCamera.SetActive(false);
        }

        _lastActiveCamera = _cameras[cameraNumber];
        _lastActiveCamera.SetActive(true);
    }

    private void Update()
    {
        if (!isClient) { return; }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCamera(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetCamera(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetCamera(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetCamera(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetCamera(4);
        }
    }
}