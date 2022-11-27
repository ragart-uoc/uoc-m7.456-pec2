using UnityEngine;

namespace PEC2.Managers
{
    /// <summary>
    /// Class <c>DeadZoneManager</c> contains the methods and properties needed for the deadzone.
    /// </summary>
    public class DeadZoneManager : MonoBehaviour
    {
        /// <summary>
        /// Method <c>OnCollisionEnter2D</c> is sent when another object enters a trigger collider attached to this object.
        /// </summary>
        /// <param name="collision">The collision instance</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject.TryGetComponent(out PlayerManager playerManager))
                {
                    playerManager.DieDirectly();
                }
            } else if (collision.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
            }
        }

    }
}
