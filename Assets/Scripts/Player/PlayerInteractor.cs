using SimpleInventory.Inputs;
using SimpleInventory.Interaction;
using UnityEngine;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace SimpleInventory.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        private const string INTERACTION_KEY = "E";

        [SerializeField] private Camera mainCamera;
        [SerializeField] private float interactionMaxDistance = 10f;
        [SerializeField] private LayerMask rayMask;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private TextMeshProUGUI interactionText;
        [SerializeField] private CanvasGroup interactionCanvasGroup;
        
        private bool previouslyDetectsInteractable = false;
        private bool detectsInteractable = false;
        private IPlayerInputsProvider playerInputsProvider;
        private IInteractable currentInteractable;

        private void Awake()
        {
            SetInteractionTextState(false);
        }

        private void Update()
        {
            if (playerInputsProvider == null)
            {
                return;
            }

            if (playerInputsProvider.InventoryKey && currentInteractable != null)
            {
                currentInteractable.Interact();
                SetInteractionTextState(false);
                currentInteractable = null;
            }
        }

        private void FixedUpdate()
        {
            if (mainCamera == null)
            {
                return;
            }

            ProcessInteractions();
        }

        private void ProcessInteractions()
        {
            var detectionRay = mainCamera.ScreenPointToRay(new (Screen.width * 0.5f, Screen.height * 0.5f, 0f));
            SetDetectedState(IsDetectingInteractable(detectionRay, out currentInteractable));
        }

        private bool IsDetectingInteractable(Ray detectionRay, out IInteractable interactable)
        {
            interactable = null;

            if (Physics.Raycast(detectionRay, out var hitInfo, interactionMaxDistance, rayMask))
            {
                return IsObjectInteractable(hitInfo.collider.gameObject.layer) &&  hitInfo.collider.gameObject.TryGetComponent(out interactable);
            }

            return false;
        }

        private void SetDetectedState(bool detectionState)
        {
            if (previouslyDetectsInteractable != detectionState)
            {
                previouslyDetectsInteractable = detectsInteractable;
                detectsInteractable = detectionState;
                SetInteractionTextState(detectionState);
            }
        }

        private void SetInteractionTextState(bool newTextEnabledState)
        {
            interactionCanvasGroup.alpha = newTextEnabledState ? 1f : 0f;
            interactionText.enabled = newTextEnabledState;

            if (newTextEnabledState && currentInteractable != null)
            {
                interactionText.text = $"Press '{INTERACTION_KEY}' to {currentInteractable.InteractionName}";
            }
        }

        private bool IsObjectInteractable(int layer)
        {
            return interactableLayer == (interactableLayer | (1 << layer));
        }
    }
}
