using UnityEngine;
using PEC2.Controllers;

namespace PEC2.Blocks
{
    /// <summary>
    /// Class <c>BreakableBlock</c> contains the methods and properties needed for the breakable blocks.
    /// </summary>
    public class BreakableBlock : Block
    {
        /// <summary>
        /// Method <c>OnCollisionEnter2D</c> is sent when an incoming collider makes contact with this object's collider.
        /// </summary>
        /// <param name="collision">The collision instance</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y > 0)
            {
                if (hits <= 0)
                    return;
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                hits--;
                StartCoroutine(hits < 1 && player.isBig ? Break() : Bounce());
            }
        }
    }
}
