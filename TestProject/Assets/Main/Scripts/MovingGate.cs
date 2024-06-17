using UnityEngine;
using Mirror;


public class MovingGate : NetworkBehaviour
{
    [SerializeField] private float _movementDistance = 5f;
    [SerializeField] private float _movementSpeed = 2f;

    private Vector3 _destination;


    private void Start()
    {
        _destination += transform.forward * _movementDistance + Vector3.up * transform.position.y;
    }

    private void Update()
    {
        Vector3 direction = _destination - transform.position;
        transform.position += direction.normalized * _movementSpeed * Time.deltaTime;

        if (direction.magnitude < 0.5f)
        {
            SetDirection();
        }
    }

    private void SetDirection()
    {
        _destination.z *= -1;
    }
}