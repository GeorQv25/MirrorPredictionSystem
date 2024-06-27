using UnityEngine;


public class KeyboardController : ControllerBase
{
    public KeyboardController(Transform relativeTransform, Rigidbody rigidbody, float speed) : base(relativeTransform, rigidbody, speed)
    {

    }

    public override void SetDirection()
    {
        _moveDir = _relativeTransform.forward * Input.GetAxisRaw("Vertical") + _relativeTransform.right * Input.GetAxisRaw("Horizontal");
    }
}