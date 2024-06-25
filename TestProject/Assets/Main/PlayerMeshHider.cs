using Mirror;
using UnityEngine;


public class PlayerMeshHider : NetworkBehaviour
{
    [SerializeField] private MeshRenderer[] _renderers;
    [SerializeField] private bool _shouldHide;


    public override void OnStartLocalPlayer()
    {
        if (!_shouldHide) { return; }

        foreach (var renderer in _renderers)
        {
            renderer.enabled = false;
        }
    }
}