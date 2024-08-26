using SimpleInventory.Inputs;
using UnityEngine;

namespace SimpleInventory.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const float COLLISION_ERROR_MARGIN = 0.01f;
        private const float GROUND_CHECK_HEIGHT = 0.001f;
        private const float GROUND_CHECK_HORIZONTAL_SIZE_MULTIPLIER = 0.4f;
        private const float GROUND_CHECK_REALTIVE_OFFSET_Y = 0.05f;

        public bool IsGrounded { get; private set; } = false;

        [SerializeField] private float movementSpeed = 10.0f;
        [SerializeField] private float acceleration = 10.0f;
        [SerializeField] private float minRotationX = -89f;
        [SerializeField] private float maxRotationX = 89f;
        [SerializeField] private float rotationSensitivity = 0.1f;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Collider playerCollider;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private LayerMask groundMask;

        private IPlayerInputsProvider playerInputsProvider;
        private Vector2 viewAngles;
        private Vector3 movementInput;

        public void ProcessMovement()
        {
            if (rigidbody == null || playerCollider == null)
            {
                return;
            }

            ApplyRotation(FetchRotation());
            ApplyMovement(GetWorldInputForce());
        }

        private void FixedUpdate()
        {
            CheckGrounded();
            ProcessMovement();
        }

        private void Update()
        {
            if (playerInputsProvider == null)
            {
                return;
            }

            movementInput = Vector3.right * playerInputsProvider.Movement.x + Vector3.forward * playerInputsProvider.Movement.y;
        }

        private void Awake()
        {
            playerInputsProvider = new PlayerInputsProvider();
        }

        private void CheckGrounded()
        {
            IsGrounded = Physics.CheckBox(
             transform.position - (Vector3.up * GROUND_CHECK_REALTIVE_OFFSET_Y),
              new Vector3(playerCollider.bounds.size.x * GROUND_CHECK_HORIZONTAL_SIZE_MULTIPLIER - COLLISION_ERROR_MARGIN, GROUND_CHECK_HEIGHT, playerCollider.bounds.size.z * GROUND_CHECK_HORIZONTAL_SIZE_MULTIPLIER - COLLISION_ERROR_MARGIN),
              transform.rotation, groundMask, QueryTriggerInteraction.Ignore);
        }

        private void ApplyMovement(Vector3 movementDirection)
        {
            var defaultVelocity = movementDirection * movementSpeed;
            var fixedVelocity = Vector3.MoveTowards(rigidbody.velocity, defaultVelocity, Time.deltaTime * acceleration);

            rigidbody.velocity = new Vector3(fixedVelocity.x, rigidbody.velocity.y, fixedVelocity.z);
        }

        private Vector3 GetWorldInputForce()
        {
            var worldInputDirection = Vector3.ClampMagnitude(transform.rotation * movementInput, 1f);

            if (IsGrounded)
            {
                var projectedInputDirection = Vector3.ProjectOnPlane(worldInputDirection, transform.up);
                projectedInputDirection = projectedInputDirection.normalized * worldInputDirection.magnitude;
                return projectedInputDirection;
            }
            else
            {
                return worldInputDirection;
            }
        }

        private Vector2 FetchRotation()
        {
            var rotation = Vector2.zero;
            var finalSensitivity = rotationSensitivity;
            rotation.x += playerInputsProvider.MouseDelta.y * finalSensitivity * -1f;
            rotation.y += playerInputsProvider.MouseDelta.x * finalSensitivity;
            return rotation;
        }

        private void ApplyRotation(Vector2 rotation)
        {
            viewAngles += rotation;
            viewAngles.x = Mathf.Clamp(Mathf.DeltaAngle(0, viewAngles.x), minRotationX, maxRotationX);

            playerCamera.transform.localRotation = Quaternion.Euler(new(viewAngles.x, 0, 0));
            rigidbody.MoveRotation(Quaternion.Euler(new(0, viewAngles.y, 0)));
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (playerCollider == null)
            {
                return;
            }

            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position - (Vector3.up * GROUND_CHECK_REALTIVE_OFFSET_Y),
            new Vector3(playerCollider.bounds.size.x * GROUND_CHECK_HORIZONTAL_SIZE_MULTIPLIER - COLLISION_ERROR_MARGIN, GROUND_CHECK_HEIGHT, playerCollider.bounds.size.z * GROUND_CHECK_HORIZONTAL_SIZE_MULTIPLIER - COLLISION_ERROR_MARGIN));
        }
        #endif
    }
}

