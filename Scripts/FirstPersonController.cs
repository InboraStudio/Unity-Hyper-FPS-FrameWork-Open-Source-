using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/First Person Controller")]
    [RequireComponent(typeof(CharacterManager))]
    [RequireComponent(typeof(CharacterController), typeof(CharacterInput), typeof(PlayerInput))]
    public class FirstPersonController : MonoBehaviour, ICharacterController
    {
        [Header("Movement")]
        [Tooltip("The amount of time needed to walk or sprint in full speed.")]
        public float acceleration = 0.1f;
        [Tooltip("The amount of meters to move per second while walking.")]
        public float walkSpeed = 5;
        [Tooltip("The amount of meters to move per second while sprinting.")]
        public float sprintSpeed = 10;
        [Tooltip("The amount of meters to move per second while tactical walking.")]
        public float tacticalSprintSpeed = 11;
        [Tooltip("The amount of force applied when jumping.")]
        public float jumpHeight = 6;
        [Tooltip("Player height while crouching.")]
        public float crouchHeight = 1.5f;
        [Tooltip("The amount of update calles in order to perform one step.")]
        public float stepInterval = 7;

        [Header("Slopes")]
        public bool slideDownSlopes = true;
        public float slopeSlideSpeed = 1;

        [Space]
        [Tooltip("Force multiplier from Physics/Gravity.")]
        public float gravity = 1;
        [Tooltip("Max speed the player can reach while falling")]
        public float maxFallSpeed = 350;
        [Tooltip("Force multiplier from Physics/Gravity when grounded")]
        public float stickToGroundForce = 0.5f;

        [Header("Camera")]
        [Tooltip("Camera or camera holder which will rotate when rotating view.")]
        public Transform _Camera;
        [Tooltip("Sensitivity of camera movement.")]
        public float sensitivity = 200;
        [Tooltip("Max angle of view rotation.")]
        public float maximumX = 90f;
        [Tooltip("Min angle of view rotation.")]
        public float minimumX = -90f;
        [Tooltip("Camera offset from the player.")]
        public Vector3 offset = new Vector3(0, -0.2f, 0);
        [Tooltip("Changes camera sens dynamicly change with camera field of view.")]
        public bool dynamicSensitivity = true;
        [Tooltip("Locks and reset cursor on start")]
        public bool lockCursor = true;
        public bool globalOrientation = false;

        [Header("Audio")]
        [Tooltip("(optional) Footsteps list to play a random sound clip from while walking.")]
        public AudioProfile[] footstepsSFX;
        [Tooltip("(optional) Sound of jumping.")]
        public AudioProfile jumpSFX;
        [Tooltip("(optional) Sound of landing.")]
        public AudioProfile landSFX;

        public CollisionFlags CollisionFlags { get; set; }
        public CharacterController controller { get; set; }
        public CameraManager cameraManager { get; set; }
        public Actor Actor { get; set; }
        public CharacterManager characterManager { get; set; }

        public CharacterInput CharacterInput { get; private set; }
        public PlayerInput playerInput { get; set; }

        //input velocity
        private Vector3 desiredVelocityRef;
        private Vector3 desiredVelocity;
        private Vector3 slideVelocity;

        //out put velocity
        private Vector3 velocity;
        
        public Transform Orientation { get; set; }
        public float tacticalSprintAmount { get; set; }
        public bool canTacticalSprint { get; set; }
        float ICharacterController.sprintSpeed { get => sprintSpeed; }
        float ICharacterController.walkSpeed { get => walkSpeed; }
        float ICharacterController.tacticalSprintSpeed { get => tacticalSprintSpeed; }
        float ICharacterController.sensitivity { get => sensitivity; }

        private Vector3 slopeDirection;

        private float yRotation;
        private float xRotation;

        private float speed;
        private float outputWalkSpeed;
        private float outputSprintSpeed;
        private float outputTacticalSprintSpeed;
        private float defaultHeight;
        private float defaultstepOffset;

        private float stepCycle;
        private float nextStep;

        private Audio footStepsAudio = new Audio();
        private Audio jumpAudio = new Audio();
        private Audio landAudio = new Audio();

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            playerInput = GetComponent<PlayerInput>();
            CharacterInput = GetComponent<CharacterInput>();
            Actor = GetComponent<Actor>();
            cameraManager = GetComponentInChildren<CameraManager>();

            footStepsAudio.Equip(gameObject, null);
            jumpAudio.Equip(gameObject, jumpSFX);
            landAudio.Equip(gameObject, landSFX);

            if (GetComponentInChildren<Inventory>()) GetComponentInChildren<Inventory>().characterManager = characterManager;

            if (transform.Find("Orientation") != null)
            {
                Orientation = transform.Find("Orientation");
            }
            else
            {
                Orientation = new GameObject("Orientation").transform;
                Orientation.parent = transform;
                Orientation.localPosition = Vector3.zero;
                Orientation.localRotation = Quaternion.identity;
            }

            characterManager.orientation = Orientation;
            characterManager.Setup(Actor, controller, cameraManager, Orientation);
        }

        protected virtual void Start()
        {
            if (!_Camera) _Camera = GetComponentInChildren<Camera>().transform;

            //setup nesscary values
            controller = GetComponent<CharacterController>();

            ResetSpeed();

            //get defaults
            defaultHeight = controller.height;
            defaultstepOffset = controller.stepOffset;
            controller.skinWidth = controller.radius / 10;

            //hide and lock cursor if there is no pause menu in the scene
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (cameraManager)
            {
                cameraManager.characterManager = characterManager;
            }

            characterManager.onLand.AddListener(PlayLandSFX);
        }


        protected virtual void Update()
        {
            //slide down slope if on maxed angle slope
            if (slideDownSlopes && OnMaxedAngleSlope())
                slideVelocity += new Vector3(slopeDirection.x, -slopeDirection.y, slopeDirection.z) * slopeSlideSpeed * Time.deltaTime;
            else
                //reset velocity if not on slope
                slideVelocity = Vector3.zero;
            //update desiredVelocity in order to normlize it and smooth the movement
            desiredVelocity = slideVelocity + Vector3.SmoothDamp(desiredVelocity,
                (SlopeDirection() * CharacterInput.moveInput.y + Orientation.right * CharacterInput.moveInput.x).normalized * speed, ref desiredVelocityRef, acceleration);

            //set controller height according to if player is crouching
            controller.height = CharacterInput.crouchInput ?
            Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * 15) :
            Mathf.Lerp(controller.height, defaultHeight, Time.deltaTime * 15);

            if (!controller.isGrounded || OnSlope())
            {
                controller.stepOffset = 0;
            }
            else
            {
                controller.stepOffset = defaultstepOffset;
            }

            //copy desiredVelocity x, z with normlized values
            velocity.x = (desiredVelocity.x);
            velocity.z = (desiredVelocity.z);

            //update speed according to if player is holding sprint
            if (CharacterInput.sprintInput && !CharacterInput.tacticalSprintInput) speed = outputSprintSpeed;
            else if (!CharacterInput.tacticalSprintInput) speed = outputWalkSpeed;

            if (CharacterInput.tacticalSprintInput) speed = outputTacticalSprintSpeed;

            //update gravity and jumping
            if (controller.isGrounded)
            {
                //set small force when grounded in order to staplize the controller
                velocity.y = Physics.gravity.y * stickToGroundForce;

                //check jumping input
                if (CharacterInput.jumpInput)
                {
                    //update velocity in order to jump
                    velocity += jumpHeight * Vector3.up + (-Physics.gravity * gravity * stickToGroundForce);

                    //play jump sound
                    if (jumpSFX)
                        jumpAudio.PlayOneShot(jumpSFX);
                }
            }
            else if (velocity.magnitude * 3.5f < maxFallSpeed)
            {
                //add gravity
                velocity += Physics.gravity * gravity * Time.deltaTime;
            }

            //move and update CollisionFlags in order to check if collition is coming from above ot center or bottom
            CollisionFlags = controller.Move(velocity * Time.deltaTime);

            //move camera according to controller height
            _Camera.position = transform.position + ((transform.up * controller.height / 2) + offset);

            //rotate camera
            UpdateCameraRotation();
            tacticalSprintAmount = CharacterInput.tacticalSprintInput ? 1 : 0;
        }

        public float GetAverageSpeed()
        {
            return (walkSpeed + sprintSpeed + tacticalSprintSpeed) / 3;
        }

        public virtual void PlayLandSFX()
        {
            if (landSFX)
                landAudio.PlayOneShot(landSFX);
        }

        public virtual void FixedUpdate()
        {
            //update step sounds
            ProgressStepCycle(speed);
        }

        protected virtual void ProgressStepCycle(float speed)
        {
            //stop if not grounded
            if (!controller.isGrounded || footstepsSFX.Length <= 0) return;

            //check if taking input and input
            if (controller.velocity.sqrMagnitude > 0 && (CharacterInput.moveInput.x != 0 || CharacterInput.moveInput.y != 0))
            {
                //update step cycle
                stepCycle += (controller.velocity.magnitude + (speed * (!characterManager.IsVelocityZero() ? 1f : 1))) * Time.fixedDeltaTime;
            }

            //check step cycle not equal to next step in order to update right
            if (!(stepCycle > nextStep))
            {
                return;
            }

            //update
            nextStep = stepCycle + stepInterval;

            if (footstepsSFX != null)
            {
                AudioProfile audioProfile = footstepsSFX[Random.Range(0, footstepsSFX.Length)];
                footStepsAudio.Update(audioProfile);
                footStepsAudio.PlayOneShot(audioProfile);
            }
        }

        protected virtual void UpdateCameraRotation()
        {
            if (prevCamRotation != _Camera.rotation) OnCameraRotationUpdated();

            yRotation += CharacterInput.lookInput.x;
            xRotation -= CharacterInput.lookInput.y;

            xRotation = Mathf.Clamp(xRotation, minimumX, maximumX);
            Quaternion cameraRotation = Quaternion.Euler(xRotation, yRotation, 0);
            Quaternion playerRotation = Quaternion.Euler(0, yRotation, 0);

            Orientation.SetRotation(playerRotation, !globalOrientation);
            _Camera.SetRotation(cameraRotation, !globalOrientation);

            prevCamRotation = _Camera.rotation;
        }

        private Quaternion prevCamRotation;

        protected virtual void OnCameraRotationUpdated() { }

        public virtual bool OnSlope()
        {
            //check if slope angle is more than 0
            if (SlopeAngle() > 0)
            {
                return true;
            }

            return false;
        }

        public virtual bool OnMaxedAngleSlope()
        {
            if (controller.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, controller.height))
            {
                slopeDirection = hit.normal;
                return Vector3.Angle(slopeDirection, Vector3.up) > controller.slopeLimit;
            }

            return false;
        }

        public virtual Vector3 SlopeDirection()
        {
            //setup a raycast from position to down at the bottom of the collider
            RaycastHit slopeHit;
            if (Physics.Raycast(Orientation.position, Vector3.down, out slopeHit, (controller.height / 2) + 0.1f))
            {
                //get the direction result according to slope normal
                return Vector3.ProjectOnPlane(Orientation.forward, slopeHit.normal);
            }

            //if not on slope then slope is forward ;)
            return Orientation.forward;
        }

        public virtual float SlopeAngle()
        {
            //setup a raycast from position to down at the bottom of the collider
            RaycastHit slopeHit;
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit))
            {
                //get the direction result according to slope normal
                return (Vector3.Angle(Vector3.down, slopeHit.normal) - 180) * -1;
            }

            //if not on slope then slope is forward ;)
            return 0;
        }

        public virtual void SetSpeed(float walk, float sprint, float tacSprint)
        {
            outputWalkSpeed = walk;
            outputSprintSpeed = sprint;
            outputTacticalSprintSpeed = tacSprint;
        }

        public virtual void ResetSpeed()
        {
            outputWalkSpeed = walkSpeed;
            outputSprintSpeed = sprintSpeed;
            outputTacticalSprintSpeed = tacticalSprintSpeed;
        }

        public virtual bool MaxedCameraRotation()
        {
            return xRotation < -90 + 1 || xRotation > 90 - 1;
        }

        protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //if hit something while jumping from the above then go down again
            if (CollisionFlags == CollisionFlags.Above)
            {
                velocity.y = 0;
            }
        }
    }
}