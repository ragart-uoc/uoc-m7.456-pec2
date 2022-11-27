using System.Collections;
using PEC2.Managers;
using UnityEngine;

namespace PEC2.Blocks
{
    /// <summary>
    /// Class <c>Block</c> contains the general methods and properties needed for the blocks.
    /// </summary>
    public abstract class Block : MonoBehaviour
    {
        /// <value>Property <c>particlePrefab</c> represents the particle prefab.</value>
        [SerializeField] private GameObject particlePrefab;

        /// <value>Property <c>altSprite</c> represents the alternative sprite that will be shown when hits are exhausted.</value>
        [SerializeField] private Sprite altSprite;
        
        /// <value>Property <c>powerUpPrefab</c> represents the powerUp prefab.</value>
        [SerializeField] protected GameObject powerUpPrefab;

        /// <value>Property <c>hits</c> represents the number of hits the block can take.</value>
        [SerializeField] protected int hits = 1;

        /// <value>Property <c>AudioSource</c> represents the AudioSource component of the block.</value>
        protected AudioSource AudioSource;
        
        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Method <c>Bounce</c> makes the breakable block bounce.
        /// </summary>
        /// <returns>IEnumerator</returns>
        protected IEnumerator Bounce()
        {
            AudioSource.PlayOneShot(GameplayManager.Instance.AudioClips.TryGetValue("blockBounceSound", out AudioClip clip) ? clip : null);
            var transform1 = transform;
            var position = transform1.position;

            transform1.position = new Vector3(position.x, position.y + 0.1f, position.z);
            yield return new WaitForSeconds(0.1f);
            transform1.position = new Vector3(position.x, position.y, position.z);
            yield return new WaitForSeconds(0.25f);

            if (powerUpPrefab == null) yield break;
            
            var powerUpPosition = new Vector3(position.x, position.y + 1f, position.z);
            var powerUp = Instantiate(powerUpPrefab, powerUpPosition, Quaternion.identity);
            if (powerUp.name == "Coin(Clone)")
            {
                AudioSource.PlayOneShot(GameplayManager.Instance.AudioClips.TryGetValue("coinSound", out AudioClip coinClip) ? coinClip : null);
                GameplayManager.Instance.AddRings(1);
                yield return new WaitForSeconds(0.5f);
                Destroy(powerUp);
            }
            else
            {
                AudioSource.PlayOneShot(GameplayManager.Instance.AudioClips.TryGetValue("powerUpSound", out AudioClip powerUpClip) ? powerUpClip : null);
            }
        }

        /// <summary>
        /// Method <c>Break</c> breaks the block.
        /// </summary>
        /// <returns>IEnumerator</returns>
        protected IEnumerator Break()
        {
            AudioSource.PlayOneShot(GameplayManager.Instance.AudioClips.TryGetValue("blockBreakSound", out AudioClip clip) ? clip : null);
            yield return new WaitForSeconds(0.1f);
            var position = transform.position;
            Instantiate(particlePrefab, new Vector3(position.x, position.y, particlePrefab.transform.position.z),
                Quaternion.identity);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }

        /// <summary>
        /// Method <c>SwitchSprite</c> changes the sprite of the block.
        /// </summary>
        /// <param name="bounce">Whether or not the block should bounce after switching sprites.</param>
        /// <returns>IEnumerator</returns>
        protected IEnumerator SwitchSprite(bool bounce = false)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = altSprite;
            yield return new WaitForSeconds(0.1f);
            if (bounce)
            {
                StartCoroutine(Bounce());
            }
        }

    }
}
