using UnityEngine;

namespace PEC2.Controllers
{
    /// <summary>
    /// Class <c>PlayerController</c> contains the methods and properties needed to control the player.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <value>Property <c>runSpeed</c> defines the initial speed of the player.</value>
        public float runSpeed = 8f;

        /// <value>Property <c>maxSpeed</c> defines the maximum speed of the player.</value>
        public float maxSpeed = 16f;

        /// <value>Property <c>jumpForce</c> defines the jump force of the player.</value>
        public float jumpHeight = 5f;

        /// <value>Property <c>groundLayer</c> represents the LayerMask component of the player.</value>
        public LayerMask groundLayer;

        /// <value>Property <c>_body</c> represents the RigidBody2D component of the player.</value>
        private Rigidbody2D _body;

        /// <value>Property <c>_renderer</c> represents the SpriteRenderer component of the player.</value>
        private SpriteRenderer _renderer;

        /// <value>Property <c>_animator</c> represents the Animator component of the player.</value>
        private Animator _animator;

        /// <value>Property <c>_speed</c> represents the horizontal speed of the player.</value>
        private float _speed;

        /// <value>Property <c>_isBig</c> represents the size of the player.</value>
        public bool isBig;

        /// <value>Property <c>_isJumping</c> defines if the player is jumping.</value>
        private bool _isJumping;
        
        /// <value>Property <c>_isCrouching</c> defines if the player is crouching.</value>
        private bool _isCrouching;

        /// <value>Property <c>AnimatorSpeed</c> preloads the Animator Speed parameter.</value>
        private static readonly int AnimatorSpeed = Animator.StringToHash("Speed");

        /// <value>Property <c>AnimatorIsGrounded</c> preloads the Animator isGrounded parameter.</value>
        private static readonly int AnimatorIsGrounded = Animator.StringToHash("isGrounded");

        /// <value>Property <c>AnimatorIsMoving</c> preloads the Animator isMoving parameter.</value>
        private static readonly int AnimatorIsMoving = Animator.StringToHash("isMoving");

        /// <value>Property <c>AnimatorIsJumping</c> preloads the Animator isJumping parameter.</value>
        private static readonly int AnimatorIsJumping = Animator.StringToHash("isJumping");
        
        /// <value>Property <c>AnimatorIsCrouching</c> preloads the Animator isCrouching parameter.</value>
        private static readonly int AnimatorIsCrouching = Animator.StringToHash("isCrouching");

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Method <c>Update</c> is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            _speed = Input.GetAxisRaw("Horizontal") * runSpeed;
            _animator.SetFloat(AnimatorSpeed, Mathf.Abs(_speed));
            _animator.SetBool(AnimatorIsMoving, _body.velocity.x > 0.1f);

            if (Input.GetKey(KeyCode.RightArrow))
            {
                _renderer.flipX = false;
                MoveForward();
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _renderer.flipX = true;
                MoveBackward();
            }

            if (Input.GetKey(KeyCode.UpArrow) && IsGrounded())
            {
                _isJumping = true;
            }
            
            if (Input.GetKey(KeyCode.DownArrow) && IsGrounded())
            {
                _isCrouching = true;
            }
            
        }

        /// <summary>
        /// Method <c>FixedUpdate</c> is called every fixed frame-rate frame.
        /// </summary>
        private void FixedUpdate()
        {
            // Clamp the maximum speed of the player
            _body.velocity = Vector2.ClampMagnitude(_body.velocity, maxSpeed);

            // Make the player jump
            if (_isJumping)
            {
                _isJumping = false;
                _animator.SetBool(AnimatorIsJumping, true);
                float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * _body.gravityScale));
                _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            
            // Make the player crouch
            if (_isCrouching)
            {
                _isCrouching = false;
                _animator.SetBool(AnimatorIsCrouching, true);
            }
            else
            {
                _animator.SetBool(AnimatorIsCrouching, false);
            }
        }

        /// <summary>
        /// Method <c>OnCollisionEnter2D</c> is sent when an incoming collider makes contact with this object's collider.
        /// </summary>
        /// <param name="collision">The collision instance</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") && IsGrounded())
            {
                _animator.SetBool(AnimatorIsJumping, false);
                _animator.SetBool(AnimatorIsGrounded, true);
            }
        }

        /// <summary>
        /// Method <c>OnCollisionStay2D</c> is sent each frame where a collider on another object is touching this object's collider.
        /// </summary>
        /// <param name="collision">The collision instance</param>
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                if (IsGrounded())
                {
                    _animator.SetBool(AnimatorIsJumping, false);
                    _animator.SetBool(AnimatorIsGrounded, true);
                }
                else
                {
                    _animator.SetBool(AnimatorIsGrounded, false);
                    _animator.SetBool(AnimatorIsCrouching, false);
                }
            }
        }

        /// <summary>
        /// Method <c>OnCollisionExit2D</c> is sent when a collider on another object stops touching this object's collider.
        /// </summary>
        /// <param name="collision">The collision instance</param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") && !IsGrounded())
            {
                _animator.SetBool(AnimatorIsGrounded, false);
                _animator.SetBool(AnimatorIsCrouching, false);
            }
        }

        /// <summary>
        /// Method <c>MoveForward</c> moves the player forward.
        /// </summary>
        private void MoveForward()
        {
            var right = transform.right;
            _body.velocity += new Vector2(right.x * runSpeed, right.y * runSpeed) * Time.deltaTime;
        }

        /// <summary>
        /// Method <c>MoveBackward</c> moves the player backward.
        /// </summary>
        private void MoveBackward()
        {
            var right = transform.right;
            _body.velocity -= new Vector2(right.x * runSpeed, right.y * runSpeed) * Time.deltaTime;
        }

        private bool IsGrounded()
        {
            Vector2 position = transform.position;
            Vector2 direction = Vector2.down;
            float distance = 0.7f;

            Debug.DrawRay(position, direction, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
            if (hit.collider != null)
            {
                return true;
            }

            return false;
        }
    }
}
