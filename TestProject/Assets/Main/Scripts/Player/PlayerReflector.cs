using System.Collections;
using System.Collections.Generic;
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
        if (collision.gameObject.TryGetComponent(out GhostBallPredictedRB ball)) ball.PushBall(direction * force);
    }
}
