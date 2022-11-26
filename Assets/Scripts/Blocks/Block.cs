using System.Collections;
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

        /// <value>Property <c>hits</c> represents the number of hits the block can take.</value>
        [SerializeField] protected int hits = 1;

        /// <summary>
        /// Method <c>Bounce</c> makes the breakable block bounce.
        /// </summary>
        /// <returns>IEnumerator</returns>
        protected IEnumerator Bounce()
        {
            var transform1 = transform;
            var position = transform1.position;

            transform1.position = new Vector3(position.x, position.y + 0.1f, position.z);
            yield return new WaitForSeconds(0.1f);
            transform1.position = new Vector3(position.x, position.y, position.z);
            yield return new WaitForSeconds(0.25f);
        }

        /// <summary>
        /// Method <c>Break</c> breaks the block.
        /// </summary>
        /// <returns>IEnumerator</returns>
        protected IEnumerator Break()
        {
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
