using System.Collections;
using UnityEngine;

namespace PEC2.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        /// <value>Property <c>movingSpeed</c> defines the initial speed of the enemy.</value>
        public float movingSpeed = 1f;
        
        /// <value>Property <c>Directions</c> defines a list of possible directions.</value>
        public enum Directions { Left, Right };
        
        /// <value>Property <c>direction</c> defines the current direction of the enemy.</value>
        public Directions direction;
        
        /// <value>Property <c>_body</c> represents the RigidBody2D component of the enemy.</value>
        private Rigidbody2D _body;
        
        /// <value>Property <c>_renderer</c> represents the SpriteRenderer component of the player.</value>
        private SpriteRenderer _renderer;

        /// <value>Property <c>_animator</c> represents the Animator component of the player.</value>
        private Animator _animator;

        /// <value>Property <c>AnimatorIsDead</c> preloads the Animator isDead parameter.</value>
        private static readonly int AnimatorIsDead = Animator.StringToHash("isDead");

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            enabled = false;
            _body = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Method <c>Update</c> is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            _body.velocity = direction == Directions.Left ? new Vector2(-movingSpeed, _body.velocity.y) : new Vector2(movingSpeed, _body.velocity.y);
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
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.TryGetComponent(out PlayerManager playerManager))
                {
                    playerManager.GetHit();
                }
            }
            else if (!collision.gameObject.CompareTag("Ground"))
            {
                direction = direction == Directions.Left ? Directions.Right : Directions.Left;
                _renderer.flipX = direction == Directions.Left;
            }
        }
        
        public void GetHit()
        {
            _animator.SetBool(AnimatorIsDead, true);
            enabled = false;
            StartCoroutine(Die());
        }
        
        private IEnumerator Die()
        {
            yield return new WaitForSeconds(1f);
            
            gameObject.layer = LayerMask.NameToLayer("Death");
            _renderer.sortingLayerName = "Death";
        }
    }
}
