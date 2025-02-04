using UnityEngine;

namespace PEC2.Managers
{
    /// <summary>
    /// Class <c>CameraManager</c> contains the methods and properties needed for the camera.
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        /// <value>Property <c>player</c> represents the Transform component of the player.</value>
        public Transform player;

        /// <value>Property <c>_offset</c> represents the offset between the player position and the camera position.</value>
        private Vector3 _offset;

        /// <summary>
        /// Method <c>Start</c> is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            _offset = player.position - transform.position;
        }

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            AddMainCameraColliders();
        }

        /// <summary>
        /// Method <c>Update</c> is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            // Stored built-in component in variable to reduce cost
            var transform1 = transform;

            // Only follow player if X is increased
            var newX = player.position.x - _offset.x;
            if (newX > transform1.position.x)
                transform1.position = new Vector3(player.position.x - _offset.x, 0, transform1.position.z);
        }

        /// <summary>
        /// Method <c>AddMainCameraColliders</c> adds an EdgeCollider2D component to the camera borders.
        /// </summary>
        private void AddMainCameraColliders()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null || !mainCamera.orthographic)
                return;

            var bottomLeft =
                (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(0, 0 - (mainCamera.pixelHeight / 2),
                    mainCamera.nearClipPlane));
            var bottomRight = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth,
                0 - (mainCamera.pixelHeight / 2), mainCamera.nearClipPlane));
            var topLeft =
                (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(0, mainCamera.pixelHeight * 1.5f,
                    mainCamera.nearClipPlane));
            var topRight = (Vector2)mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth,
                mainCamera.pixelHeight * 1.5f, mainCamera.nearClipPlane));

            var edge1 = GetComponent<EdgeCollider2D>() == null
                ? gameObject.AddComponent<EdgeCollider2D>()
                : GetComponent<EdgeCollider2D>();
            var edge1Points = new[] { topLeft, bottomLeft };
            edge1.points = edge1Points;

            var edge2 = gameObject.AddComponent<EdgeCollider2D>();
            var edge2Points = new[] { topRight, bottomRight };
            edge2.points = edge2Points;
        }
    }
}
