using UnityEngine;
using PEC2.Managers;

namespace PEC2.PowerUps
{
    /// <summary>
    /// Class <c>PowerUp</c> contains the general methods and properties needed for the powerUps.
    /// </summary>
    public class PowerUp : MonoBehaviour
    {

        /// <value>Property <c>movingSpeed</c> defines the initial speed of the powerUp.</value>
        public float movingSpeed = 3f;
        
        /// <value>Property <c>walkDirections</c> defines a list of possible walking directions.</value>
        public enum WalkDirections { Right, Left };
        
        /// <value>Property <c>walkDirection</c> defines the walking direction of the powerUp.</value>
        public WalkDirections walkDirection;

        /// <value>Property <c>_transform</c> represents the RigidBody2D component of the powerUp.</value>
        private Transform _transform;
        
        /// <value>Property <c>_body</c> represents the RigidBody2D component of the powerUp.</value>
        private Rigidbody2D _body;
        
        /// <value>Property <c>_renderer</c> represents the SpriteRenderer component of the powerUp.</value>
        private SpriteRenderer _renderer;

        /// <value>Property <c>_playerHasCollided</c> defines if the player has collided with the powerUp.</value>
        private bool _playerHasCollided;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _transform = transform;
            _body = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Method <c>Update</c> is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            CheckDirectionCollision();
            _body.velocity = walkDirection == WalkDirections.Left ? new Vector2(-movingSpeed, _body.velocity.y) : new Vector2(movingSpeed, _body.velocity.y);
        }
        
        /// <summary>
        /// Method <c>LateUpdate</c> is called after all Update functions have been called.
        /// </summary>
        private void LateUpdate() 
        {
            _playerHasCollided = false;
        }

        /// <summary>
        /// Method <c>OnCollisionEnter2D</c> is sent when an incoming collider makes contact with this object's collider.
        /// </summary>
        /// <param name="collision">The collision instance</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && _playerHasCollided == false)
            {
                _playerHasCollided = true;
                if (collision.gameObject.TryGetComponent(out PlayerManager playerManager)) {
                    Destroy(gameObject);
                    ActionOnPlayerCollision(playerManager);
                }
            }
        }
        
        /// <summary>
        /// Method <c>ActionOnPlayerCollision</c> defines the action that will be performed when the player collides with the powerUp.
        /// </summary>
        protected virtual void ActionOnPlayerCollision(PlayerManager playerManager)
        {
            // To be implemented in the child classes
        }

        /// <summary>
        /// Method <c>CheckDirectionCollision</c> is called every frame to change the walking direction if the enemy is colliding with something that is not the player or the camera.
        /// </summary>
        private void CheckDirectionCollision()
        {
            Vector2 position = _transform.position;
            var newPosition = new Vector2(position.x, position.y - 0.1f); 
            var direction = walkDirection == WalkDirections.Left ? Vector2.left : Vector2.right;
            var distance = _transform.localScale.x / 2 + 0.1f;
            
            var ray = new Ray2D(newPosition, direction);
            Debug.DrawRay(newPosition, direction, Color.green);
            
            var hit = Physics2D.Raycast(ray.origin, ray.direction, distance);
            if (hit.collider == null) return;
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("WeakPoint") || hit.collider.CompareTag("MainCamera")) return;

            walkDirection = walkDirection == WalkDirections.Left ? WalkDirections.Right : WalkDirections.Left;
            _renderer.flipX = (walkDirection == WalkDirections.Right);
        }
    }
}
