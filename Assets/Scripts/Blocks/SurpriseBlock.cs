using UnityEngine;

namespace PEC2.Blocks
{
    /// <summary>
    /// Class <c>SurpriseBlock</c> contains the methods and properties needed for the surprise blocks.
    /// </summary>
    public class SurpriseBlock : BouncingBlock
    {
        /// <value>Property <c>_animator</c> represents the Animator component of the block.</value>
        private Animator _animator;
        
        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Method <c>FinalState</c> is called when the block has no more hits left.
        /// </summary>
        protected override void FinalState()
        {
            _animator.enabled = false;
            StartCoroutine(SwitchSprite(true));
        }
    }
}
