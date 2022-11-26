using UnityEngine;

namespace PEC2.Blocks
{
    /// <summary>
    /// Class <c>BouncingBloc</c> contains the methods and properties needed for the bouncing blocks.
    /// </summary>
    public class BouncingBlock : Block
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
                hits--;
                if (hits < 1)
                    FinalState();
                else
                    StartCoroutine(Bounce());
            }
        }
        
        protected virtual void FinalState()
        {
            StartCoroutine(SwitchSprite(true));
        }
    }
}
