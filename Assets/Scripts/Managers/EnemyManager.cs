using System.Collections;
using UnityEngine;

namespace PEC2.Managers
{
    /// <summary>
    /// Class <c>EnemyManager</c> contains the methods and properties needed for the enemy.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        /// <value>Property <c>movingSpeed</c> defines the initial speed of the enemy.</value>
        public float movingSpeed = 1f;
        
        /// <value>Property <c>walkDirections</c> defines a list of possible walking directions.</value>
        public enum WalkDirections { Left, Right };
        
        /// <value>Property <c>walkDirection</c> defines the walking direction of the enemy.</value>
        public WalkDirections walkDirection;

        /// <value>Property <c>_transform</c> represents the RigidBody2D component of the enemy.</value>
        private Transform _transform;
        
        /// <value>Property <c>_body</c> represents the RigidBody2D component of the enemy.</value>
        private Rigidbody2D _body;
        
        /// <value>Property <c>_renderer</c> represents the SpriteRenderer component of the enemy.</value>
        private SpriteRenderer _renderer;

        /// <value>Property <c>_animator</c> represents the Animator component of the enemy.</value>
        private Animator _animator;

        /// <value>Property <c>AnimatorIsDead</c> preloads the Animator isDead parameter.</value>
        private static readonly int AnimatorIsDead = Animator.StringToHash("isDead");
        
        /// <value>Property <c>_playerHasCollided</c> defines if the player has collided with the enemy.</value>
        private bool _playerHasCollided;
        
        /// <value>Property <c>_audioSource</c> represents the AudioSource component of the enemy.</value>
        private AudioSource _audioSource;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            enabled = false;
            _transform = transform;
            _body = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            
            _audioSource = GetComponent<AudioSource>();
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
        /// Method <c>OnBecameVisible</c> is called when the renderer became visible by any camera.
        /// </summary>
        private void OnBecameVisible()
        {
            enabled = true;
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
                if (collision.gameObject.TryGetComponent(out PlayerManager playerManager))
                {
                    if (playerManager.invincibilityTime > 0)
                        GetHit();
                    else
                        playerManager.GetHit();
                }
            }
        }

        /// <summary>
        /// Method <c>GetHit</c> is called when the enemy is hit by the player.
        /// </summary>
        public void GetHit()
        {
            StartCoroutine(Die());
        }
        
        /// <summary>
        /// Method <c>Die</c> is called when the enemy is dead.
        /// </summary>
        private IEnumerator Die()
        {
            enabled = false;
            
            _audioSource.PlayOneShot(GameplayManager.Instance.AudioClips.TryGetValue("dieEnemySound", out AudioClip clip) ? clip : null);
            
            _animator.SetBool(AnimatorIsDead, true);

            _body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

            gameObject.layer = LayerMask.NameToLayer("Death");
            _transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Death");
            _renderer.sortingLayerName = "Death";
            
            yield return new WaitForSeconds(1f);
            
            _body.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
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
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("MainCamera") || hit.collider.CompareTag("PowerUp")) return;

            walkDirection = walkDirection == WalkDirections.Left ? WalkDirections.Right : WalkDirections.Left;
            _renderer.flipX = (walkDirection == WalkDirections.Right);
        }
    }
}
