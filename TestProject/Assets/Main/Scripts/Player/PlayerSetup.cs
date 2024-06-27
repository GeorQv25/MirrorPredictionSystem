using Mirror;
using UnityEngine;


public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private bool _shouldHide;
    [SerializeField] private MeshRenderer[] _renderers;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Rigidbody _rb;


    public override void OnStartLocalPlayer()
    {
        HideMeshes();
        ControllerBase input = new KeyboardController(_orientation, _rb, 4000);
        _playerController.Initialize(input);
    }

    private void HideMeshes()
    {
        if (!_shouldHide) { return; }
        
        foreach (var renderer in _renderers)
        {
            renderer.enabled = false;
        }
    }
}