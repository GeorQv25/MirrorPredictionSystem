using UnityEngine;


namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class HandCollision : MonoBehaviour
    {
        public float MaxVelocityToAttach = 1;
      
        public Rigidbody Rigidbody { get; private set; }
        [SerializeField] private Collider[] _handColliders;
        [SerializeField] private TypeHand _type;
        
        
        
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        [ContextMenu("Config Hand Collisions")]
        private void CollectCollider()
        {
            _handColliders = gameObject.GetComponentsInChildren<Collider>();
        }

        [ContextMenu("Turn off Physics")]
        private void TurnOffPhysics()
        {
            TogglePhysics(false);
        }
        
        [ContextMenu("Turn on Physics")]
        private void TurnOnPhysics()
        {
            TogglePhysics(true);
        }
        
        public void TogglePhysics(bool isEnable)
        {
            foreach (var collider in _handColliders)
            {
                collider.enabled = isEnable;
                collider.gameObject.SetActive(isEnable);
            }

            if (isEnable)
            {
                Rigidbody.WakeUp();
            }
            else
            {
                Rigidbody.Sleep();
            }
        }

        public void UpdatePhysicMaterial(PhysicMaterial physicMaterial)
        {
            foreach (var collider in _handColliders)
            {
                collider.material = physicMaterial;
            }
        }
    }
}
