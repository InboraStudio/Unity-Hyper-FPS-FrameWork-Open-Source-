using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Player/Character Manager")]
    [RequireComponent(typeof(CharacterInput))]
    public class CharacterManager : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent onJump = new UnityEvent();
        public UnityEvent onLand = new UnityEvent();

        public Actor actor { get; set; }
        public CharacterInput characterInput { get; set; }
        public CharacterController characterController { get; set; }
        public Rigidbody attachedRigidbody { get; set; }
        public CameraManager cameraManager { get; set; }
        public AudioFiltersManager audioFiltersManager { get; set; }
        public Transform orientation { get; set; }
        public ICharacterController character { get; set; }
        public Vector3 velocity { get; private set; }
        public bool isGrounded { get; set; }
        private bool previouslyGrounded;

        private void Start()
        {
            character = GetComponent<ICharacterController>();
            actor = GetComponent<Actor>();
            characterInput = GetComponent<CharacterInput>();
            characterController = GetComponent<CharacterController>();
            cameraManager = transform.SearchFor<CameraManager>();
            audioFiltersManager = transform.SearchFor<AudioFiltersManager>();
        }

        public void Setup(Actor _actor, CharacterController _characterController, CameraManager _cameraManager, Transform _orientation)
        {
            actor = _actor;
            characterController = _characterController;
            cameraManager = _cameraManager;
            orientation = _orientation;
        }

        public void Setup(Actor _actor, Rigidbody _rigidbody, CameraManager _cameraManager, Transform _orientation)
        {
            actor = _actor;
            attachedRigidbody = _rigidbody;
            cameraManager = _cameraManager;
            orientation = _orientation;
        }

        /// <summary>
        /// Sets isGrounded to value. Use this for rigidbody controllers.
        /// </summary>
        /// <param name="value"></param>
        public void UpdateGroundedState(bool value) => isGrounded = value;

        private void Update()
        {
            if (characterController)
            {
                isGrounded = characterController.isGrounded;
                velocity = characterController.velocity;
            }
            else if (attachedRigidbody) velocity = attachedRigidbody.velocity;

            if (previouslyGrounded && !isGrounded) onJump?.Invoke();
            if (!previouslyGrounded && isGrounded) onLand?.Invoke();

            previouslyGrounded = isGrounded;
        }

        public virtual void AddLookValue(float vertical, float horizontal)
        {
            characterInput.AddLookValue(new Vector2(vertical, horizontal));
        }
        
        public virtual bool IsVelocityZero()
        {
            if (characterController) return characterController.IsVelocityZero();
            if (!characterController && attachedRigidbody) return attachedRigidbody.IsVelocityZero();

            return true;
        }
    }
}