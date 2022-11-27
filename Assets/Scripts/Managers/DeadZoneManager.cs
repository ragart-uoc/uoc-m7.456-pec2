using UnityEngine;

namespace PEC2.Managers
{
    public class DeadZoneManager : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject.TryGetComponent(out PlayerManager playerManager))
                {
                    playerManager.GetHit();
                }
            } else if (collision.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
            }
        }

    }
}
