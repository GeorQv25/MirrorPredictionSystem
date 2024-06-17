using UnityEngine;


public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform cameraPos;


    private void Update()
    {
        transform.position = cameraPos.position;
    }
}
