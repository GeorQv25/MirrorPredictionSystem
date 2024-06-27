using UnityEngine;

public abstract class ControllerBase
{
    protected Transform _relativeTransform;
    protected Rigidbody _playerRb;
    protected Vector3 _moveDir;
    protected float _speed;


    public ControllerBase(Transform relativeTransform, Rigidbody rigidbody, float speed)
    {
        _relativeTransform = relativeTransform;
        _playerRb = rigidbody;
        _speed = speed;
    }

    public virtual void Move()
    {
        _playerRb.AddForce(_moveDir.normalized * _speed * Time.deltaTime, ForceMode.Acceleration);
    }

    public virtual void Rotate()
    {

    }

    public virtual void Shoot()
    {

    }

    public abstract void SetDirection();
}