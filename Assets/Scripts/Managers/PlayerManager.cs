using System.Collections;
using UnityEngine;
using PEC2.Controllers;

namespace PEC2.Managers
{
    /// <summary>
    /// Class <c>PlayerManager</c> contains the methods and properties needed for the player.
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        /// <value>Property <c>isBig</c> represents the size of the player.</value>
        [HideInInspector] public bool isBig;

        /// <value>Property <c>invincibilityTime</c> represents the time left of invincibility.</value>
        [HideInInspector] public float invincibilityTime;

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
        
        /// <value>Property <c>_audioSource</c> represents the AudioSource component of the player.</value>
        private AudioSource _audioSource;
        
        /// <value>Property <c>_cameraAudioSource</c> represents the AudioSource component of the camera.</value>
        private AudioSource _cameraAudioSource;

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
            
            _audioSource = GetComponent<AudioSource>();
            _cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            if (invincibilityTime > 0)
                invincibilityTime -= Time.deltaTime;
        }

        /// <summary>
        /// Method <c>GetHit</c> is called when the player is hit.
        /// </summary>
        public void GetHit()
        {
            StartCoroutine(isBig ? Shrink() : Die());
        }
        
        /// <summary>
        /// Method <c>DieDirectly</c> is called when the player dies directly regardless of its state.
        /// </summary>
        public void DieDirectly()
        {
            StartCoroutine(Die());
        }
        
        /// <summary>
        /// Method <c>GetBigger</c> is called when the player needs to grow.
        /// </summary>
        public void GetBigger()
        {
            StartCoroutine(Grow());
        }

        /// <summary>
        /// Method <c>Grow</c> is called when the player gets bigger.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator Grow()
        {
            isBig = true;
            
            _animator.enabled = false;
            _controller.enabled = false;
            
            _audioSource.PlayOneShot(GameplayManager.Instance.AudioClips.TryGetValue("growSound", out AudioClip clip) ? clip : null);

            _transform.localScale = new Vector3(1.5f, 1.5f, 0);
            
            var position = _transform.position;
            position = new Vector3(position.x, position.y + 0.25f, position.z);
            _transform.position = position;
            
            _body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            
            for (var i = 0; i < 5; i++)
            {
                _transform.localScale = i % 2 == 0 ? new Vector3(1.5f, 1.5f, 0) : new Vector3(1f, 1f, 0);
                yield return new WaitForSecondsRealtime(0.3f);
            }
            
            _body.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            
            _controller.enabled = true;
            _animator.enabled = true;
        }
        
        /// <summary>
        /// Method <c>Shrink</c> is called when the player is hit while big.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator Shrink()
        {
            isBig = false;
            invincibilityTime = 3f;
            
            _animator.enabled = false;
            _controller.enabled = false;
            
            _audioSource.PlayOneShot(GameplayManager.Instance.AudioClips.TryGetValue("shrinkSound", out AudioClip clip) ? clip : null);
            
            _body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            
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
        
        /// <summary>
        /// Method <c>Die</c> is called when the player is dead.
        /// </summary>
        private IEnumerator Die()
        {
            _controller.enabled = false;
            
            _cameraAudioSource.Stop();
            _audioSource.PlayOneShot(GameplayManager.Instance.AudioClips.TryGetValue("diePlayerSound", out AudioClip clip) ? clip : null);

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
            
            _body.velocity = Vector2.zero;

            yield return new WaitForSeconds(3f);
            
            GameplayManager.Instance.LoseLife();
        }
    }
}
