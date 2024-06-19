using Mirror;
using UnityEngine;

struct BallState
{
    public Vector3 position;
    public Vector3 velocity;

    public BallState(Vector3 position, Vector3 velocity)
    {
        this.position = position;
        this.velocity = velocity;
    }
}

public class GhostBallPredictedRB : NetworkBehaviour
{
    [SerializeField] private Rigidbody predictedRigidbody;
    [SerializeField] private float positionTolerance, hardCorrectionTolerance;
    [SerializeField] private float minCorrectionSpeed, maxCorrectionSpeed;
    private BallState currentServerState;
    private Rigidbody ghostRigidbody;


    public override void OnStartClient()
    {
        if (!isClientOnly) return;
        GameObject ghost = new GameObject($"Ghost {predictedRigidbody.gameObject.name}");
        ghost.transform.localScale = predictedRigidbody.transform.localScale;
        ghost.layer = predictedRigidbody.gameObject.layer;
        ghost.transform.position = predictedRigidbody.transform.position;
        PredictionUtils.MovePhysicsComponents(gameObject, ghost);
        ghostRigidbody = ghost.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isServer) SetDirty();
        else if (isClient) ClientUpdate();
    }



    private void ClientUpdate()
    {
        float distance = Vector3.Distance(currentServerState.position, predictedRigidbody.position);

        MoveGhostToServer();
        MoveBallToGhost(distance);


        if (distance >= hardCorrectionTolerance && predictedRigidbody.velocity.magnitude < 1)
            HardCorrect();
    }

    private void MoveBallToGhost(float distance)
    {
        if (distance <= positionTolerance) return;

        float speed = minCorrectionSpeed + maxCorrectionSpeed - Mathf.Clamp(distance, 0, maxCorrectionSpeed);
        predictedRigidbody.MovePosition(Vector3.Lerp(predictedRigidbody.position, ghostRigidbody.position, Time.deltaTime * speed));
    }

    private void MoveGhostToServer()
    {
        ghostRigidbody.position = currentServerState.position;
        ghostRigidbody.velocity = currentServerState.velocity;
    }

    public void PushBall(Vector3 force, Vector3 pushPos)
    {
        predictedRigidbody.velocity = Vector3.zero;
        predictedRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        predictedRigidbody.constraints = RigidbodyConstraints.None;
        predictedRigidbody.position = pushPos;
        predictedRigidbody.AddForce(force, ForceMode.Impulse);
        if (ghostRigidbody)
        {
            ghostRigidbody.position = pushPos;
            ghostRigidbody.AddForce(force, ForceMode.Impulse);
        }
    }

    public override void OnSerialize(NetworkWriter writer, bool initialState)
    {
        writer.WriteVector3(predictedRigidbody.position);
        writer.WriteVector3(predictedRigidbody.velocity);
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        Vector3 position = reader.ReadVector3();
        Vector3 velocity = reader.ReadVector3();

        OnReceivedState(new BallState(position, velocity));

        if (initialState) HardCorrect();
    }

    void OnReceivedState(BallState state)
    {
        currentServerState.position = state.position;
        currentServerState.velocity = state.velocity;
    }

    private void HardCorrect()
    {
        predictedRigidbody.position = currentServerState.position;
        predictedRigidbody.velocity = currentServerState.velocity;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if(ghostRigidbody != null) Gizmos.DrawWireSphere(ghostRigidbody.position, 1f);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        syncInterval = 0;
        syncDirection = SyncDirection.ServerToClient;
    }

    public void ResetBall()
    {
        predictedRigidbody.velocity = Vector3.zero;
        predictedRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        predictedRigidbody.constraints = RigidbodyConstraints.None;
        predictedRigidbody.position = new Vector3(0, 2, 0);
    }
}