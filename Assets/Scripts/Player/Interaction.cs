using UnityEngine;

namespace Player
{
    public class Interaction : MonoBehaviour
    {
        public static Interaction Instance;
        [SerializeField]
        private Transform _colliderOffset;
    
        [SerializeField]
        private LayerMask _interactableLayer;

        private IInteractable _interactable;

        private float _checkDistance = 0.6f;
        private Vector3 _rayCastOffset = new Vector3(0.2f, 0.2f, 0f);

        public bool IsInteracting { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            CanInteract();
        }

        public void Interact(bool interactInput)
        {
            StopInteracting();
            if (CanInteract())
            {
                if (interactInput)
                {
                    _interactable?.Interact();
                    IsInteracting = true;
                }
            }
        }

        private bool CanInteract()
        {
            Vector3 origin = transform.position + _colliderOffset.position;
            RaycastHit hit;

            if (Physics.Raycast(origin, Vector3.forward, out hit, _checkDistance, _interactableLayer))
            {
                _interactable = hit.collider.GetComponent<IInteractable>();
                if (_interactable != null)
                {
                    _interactable.ShowCanInteract(true);
                    return true;
                }
            }

            return false;
        }

        private void StopInteracting()
        {
            if (_interactable == null) return;

            _interactable.ShowCanInteract(false);
            _interactable = null;
            IsInteracting = false;
        }
    }
}
