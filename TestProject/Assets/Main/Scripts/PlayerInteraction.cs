using Mirror;
using UnityEngine;


public class PlayerInteraction : NetworkBehaviour
{
    [SerializeField] private float _pushForce = 20f;
    private GhostBallPredictedRB _ball;


    private void Start()
    {
        _ball = FindAnyObjectByType<GhostBallPredictedRB>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer) { return; }

        if (other.TryGetComponent(out GhostBallPredictedRB ball))
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;
            direction.y = 0.1f;
            direction *= _pushForce;
            PushBall(direction, _ball.transform.position);
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) { return; }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isClientOnly) _ball.ResetBall();
            CmdResetBall();
        }
    }

    #region BallPush
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
    #endregion

    #region BallReset
    [Command]
    private void CmdResetBall()
    {
        _ball.ResetBall();
        RpcResetBall();
    }

    [ClientRpc(includeOwner = false)]
    private void RpcResetBall()
    {
        if (isClientOnly) _ball.ResetBall();
    }
    #endregion
}