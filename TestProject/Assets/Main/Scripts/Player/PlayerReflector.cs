using UnityEngine;
using Mirror;


public class PlayerReflector : NetworkBehaviour
{
    private float reflectRate = 1f;
    private double lastReflectTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<GhostBallPredictedRB>()) return;
        if (lastReflectTime + reflectRate > NetworkTime.localTime) return;

        lastReflectTime = NetworkTime.localTime;
        float impulse = Mathf.Clamp(collision.relativeVelocity.magnitude, 0, 30f);
        float force = collision.contacts[0].thisCollider.GetComponent<CapsuleCollider>() ? impulse / 20 : impulse / 2;
        Vector3 direction = Vector3.Reflect(collision.rigidbody.velocity, collision.contacts[0].normal).normalized;
        collision.rigidbody.velocity = direction * force;
        if (isServer) RpcCorrectBall(collision.rigidbody.position, collision.rigidbody.velocity, collision.gameObject);
    }

    [ClientRpc(includeOwner = false)]
    private void RpcCorrectBall(Vector3 position, Vector3 velocity, GameObject ball)
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.MovePosition(position);
        rb.velocity = velocity;
    }
}
