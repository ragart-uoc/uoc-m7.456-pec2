using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using PEC2.Controllers;

namespace PEC2.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        /// <value>Property <c>_isBig</c> represents the size of the player.</value>
        public bool isBig;

        /// <value>Property <c>_transform</c> represents the Transform component of the player.</value>
        private Transform _transform;
        
        /// <value>Property <c>_body</c> represents the RigidBody2D component of the player.</value>
        private Rigidbody2D _body;
        
        /// <value>Property <c>_renderer</c> represents the SpriteRenderer component of the player.</value>
        private SpriteRenderer _renderer;

        /// <value>Property <c>_animator</c> represents the Animator component of the player.</value>
        private Animator _animator;

        /// <value>Property <c>_controller</c> represents the PlayerController component of the player.</value>
        private PlayerController _controller;

        /// <value>Property <c>AnimatorIsDead</c> preloads the Animator isDead parameter.</value>
        private static readonly int AnimatorIsDead = Animator.StringToHash("isDead");
        
        private void Start()
        {
            
            StartCoroutine(Grow());
        }
        
        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _body = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _controller = GetComponent<PlayerController>();
        }

        public void GetHit()
        {
            StartCoroutine(isBig ? Shrink() : Die());
        }

        public IEnumerator Grow()
        {
            isBig = true;
            
            _animator.enabled = false;
            _controller.enabled = false;

            _transform.localScale = new Vector3(1.5f, 1.5f, 0);
            
            var position = _transform.position;
            position = new Vector3(position.x, position.y + 0.25f, position.z);
            _transform.position = position;
            
            _body.constraints = RigidbodyConstraints2D.FreezePositionY;
            
            for (var i = 0; i < 5; i++)
            {
                _transform.localScale = i % 2 == 0 ? new Vector3(1.5f, 1.5f, 0) : new Vector3(1f, 1f, 0);
                yield return new WaitForSecondsRealtime(0.3f);
            }
            
            _body.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            
            _controller.enabled = true;
            _animator.enabled = true;
        }
        
        private IEnumerator Shrink()
        {
            isBig = false;
            
            _animator.enabled = false;
            _controller.enabled = false;
            
            _body.constraints = RigidbodyConstraints2D.FreezePositionY;
            
            for (var i = 0; i < 5; i++)
            {
                _transform.localScale = i % 2 == 0 ? new Vector3(1f, 1f, 0) : new Vector3(1.5f, 1.5f, 0);
                yield return new WaitForSecondsRealtime(0.3f);
            }

            _body.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            
            var position = _transform.position;
            position = new Vector3(position.x, position.y - 0.25f, position.z);
            _transform.position = position;
            
            _controller.enabled = true;
            _animator.enabled = true;
        }
        
        private IEnumerator Die()
        {
            _controller.enabled = false;

            _animator.SetBool(AnimatorIsDead, true);
            
            gameObject.layer = LayerMask.NameToLayer("Death");
            _renderer.sortingLayerName = "Death";
            
            yield return new WaitForSeconds(0.1f);
            
            var elapsedTime = 0f;
            var duration = 0.6f;
            var position = _transform.position;
            var newPosition = new Vector3(position.x, position.y + 3f, position.z);
            
            while (elapsedTime < duration)
            {
                _transform.position = Vector3.Lerp(position, newPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(5f);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
