using PEC2.Managers;
using UnityEngine;

namespace PEC2.PowerUps
{
    /// <summary>
    /// Class <c>Mushroom</c> contains the methods and properties needed for the mushroom powerUps.
    /// </summary>
    public class Mushroom : PowerUp
    {
        /// <summary>
        /// Method <c>ActionOnPlayerCollision</c> defines the action that will be performed when the player collides with the powerUp.
        /// </summary>
        protected override void ActionOnPlayerCollision(PlayerManager playerManager)
        {
            if (playerManager.isBig)
                GameplayManager.Instance.AddPoints(1000);
            else
                playerManager.GetBigger();
        }
    }
}
