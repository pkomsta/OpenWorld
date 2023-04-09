using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player Actions")]
        public PlayerMove PlayerMove;
        public GroundCheck GroundCheck;
        public PlayerJump PlayerJump;
        public PlayerCamera PlayerCamera;
        public ApplyGravity ApplyGravity;


       

        // animation IDs
        public int _animIDSpeed { get; private set; }
        public int _animIDGrounded { get; private set; }
        public int _animIDJump { get; private set; }
        public int _animIDFreeFall { get; private set; }
        public int _animIDMotionSpeed { get; private set; }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;


        private bool _hasAnimator;

        public bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
            
        }

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

           
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            PlayerJump.JumpAndGravity(this);
            ApplyGravity.ApplyGravityToObject(this);
            GroundCheck.GroundedCheck(this);
            PlayerMove.Move(this);
        }

        private void LateUpdate()
        {
            PlayerCamera.CameraRotation(this);
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }


        public StarterAssetsInputs GetInput() {
            return _input;
        }
        public CharacterController GetController()
        {
            return _controller;
        }
        public Animator GetAnimator() 
        { 
            return _animator; 
        }
        public GameObject GetCamera()
        {
            return _mainCamera;
        }
        public bool HasAnimator()
        {
            return _hasAnimator;
        }
    }
}