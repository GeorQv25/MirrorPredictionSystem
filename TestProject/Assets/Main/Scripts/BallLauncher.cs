using Mirror;
using UnityEngine;


public class BallLauncher : NetworkBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _cameraTransform;
    
    private float _distanceFromCamera = 2;
    private float _pushForce = 30;
    private GhostBallPredictedRB _ball;


    public void Awake()
    {
        _ball = FindAnyObjectByType<GhostBallPredictedRB>();
    }

    public void Shoot()
    {
        PushBall(_cameraTransform.forward * _pushForce, _shootPoint.position + _cameraTransform.forward * _distanceFromCamera);
    }

    private void PushBall(Vector3 direction, Vector3 ballPos)
    {
        if (isClientOnly) _ball.PushBall(direction, ballPos);
        CmdPushBall(direction, ballPos);
    }

    [Command]
    private void CmdPushBall(Vector3 direction, Vector3 ballPos)
    {
        _ball.PushBall(direction, ballPos);
        RpcPushBall(direction, ballPos);
    }

    [ClientRpc(includeOwner = false)]
    private void RpcPushBall(Vector3 direction, Vector3 ballPos)
    {
        if (isClientOnly) _ball.PushBall(direction, ballPos);
    }
}