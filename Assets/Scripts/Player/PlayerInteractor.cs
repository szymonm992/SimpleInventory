using SimpleInventory.Inputs;
using SimpleInventory.Interaction;
using UnityEngine;
using TMPro;
using Zenject;
using System;

namespace SimpleInventory.Player
{
    public class PlayerInteractor : MonoBehaviour, IInitializable, IDisposable, IFixedTickable
    {
        //TODO: This should be refactored
        private const string INTERACTION_KEY = "E";

        [Inject] private readonly IPlayerInputsProvider playerInputsProvider;
        [Inject(Id = PlayerInstaller.PLAYER_CAMERA_ID)] private readonly Camera playerCamera;

        [SerializeField] private float interactionMaxDistance = 10f;
        [SerializeField] private LayerMask rayMask;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private TextMeshProUGUI interactionText;
        [SerializeField] private CanvasGroup interactionCanvasGroup;
        
        private bool detectsInteractable = false;
        private IInteractable currentInteractable;

        public void Initialize()
        {
            SetInteractionTextState(false);
            playerInputsProvider.InteractButtonPressedEvent += OnInteractButtonPressed;
        }

        public void Dispose()
        {
            playerInputsProvider.InteractButtonPressedEvent -= OnInteractButtonPressed;
        }

        public void FixedTick()
        {
            if (playerCamera == null)
            {
                return;
            }

            ProcessInteractions();
        }

        private void OnInteractButtonPressed()
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
                currentInteractable = null;
            }
        }

        private void ProcessInteractions()
        {
            var detectionRay = playerCamera.ScreenPointToRay(new (Screen.width * 0.5f, Screen.height * 0.5f, 0f));
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
            if (detectsInteractable != detectionState)
            {
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
                interactionText.text = $"Press '{INTERACTION_KEY}' to {currentInteractable.InteractionActionName}";
            }
        }

        private bool IsObjectInteractable(int layer)
        {
            return interactableLayer == (interactableLayer | (1 << layer));
        }
    }
}
