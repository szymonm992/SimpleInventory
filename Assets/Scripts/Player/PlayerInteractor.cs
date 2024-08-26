using SimpleInventory.Inputs;
using SimpleInventory.Interaction;
using UnityEngine;

namespace SimpleInventory.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask interactableLayer;

        private bool previouslyDetectsInteractable = false;
        private bool detectsInteractable = false;
        private IPlayerInputsProvider playerInputsProvider;
        private IInteractable currentInteractable;

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

            if (Physics.Raycast(detectionRay, out var hitInfo, float.MaxValue, interactableLayer))
            {
                return hitInfo.transform.root.TryGetComponent(out interactable);
            }

            return false;
        }

        private void SetDetectedState(bool detectionState)
        {
            if (previouslyDetectsInteractable != detectionState)
            {
                previouslyDetectsInteractable = detectsInteractable;
                detectsInteractable = detectionState;
                SetInteractionTextState(detectionState, currentInteractable.InteractionName);
            }
        }

        private void SetInteractionTextState(bool state, string interactionActionText = "Interact")
        {

        }
    }
}
