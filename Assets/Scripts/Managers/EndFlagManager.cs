using System.Collections;
using UnityEngine;

namespace PEC2.Managers
{
    /// <summary>
    /// Class <c>EndFlagManager</c> contains the methods and properties needed for the end flag.
    /// </summary>
    public class EndFlagManager : MonoBehaviour
    {
        /// <value>Property <c>_cameraAudioSource</c> represents the AudioSource component of the camera.</value>
        private AudioSource _cameraAudioSource;
        
        
        
        /// <summary>
        /// Method <c>OnTriggerEnter2D</c> is sent when another object enters a trigger collider attached to this object.
        /// </summary>
        /// <param name="collision">The collision instance</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
                _cameraAudioSource.Stop();
                _cameraAudioSource.PlayOneShot(GameplayManager.Instance.AudioClips.TryGetValue("gameWinSound", out AudioClip clip) ? clip : null);
                StartCoroutine(EndGame());
            }
        }
        
        private IEnumerator EndGame()
        {
            yield return new WaitForSeconds(3f);
            GameplayManager.Instance.WinGame();
        }
    }
}
