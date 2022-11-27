using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PEC2.Managers
{
    /// <summary>
    /// Class <c>GameplayManager</c> contains the methods and properties needed for the game.
    /// </summary>
    public class GameplayManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the instance of the GameplayManager.</value>
        public static GameplayManager Instance;

        /// <value>Property <c>_lives</c> represents the lives of the player.</value>
        private int _lives = 3;
        
        /// <value>Property <c>_points</c> represents the points of the player.</value>
        private int _points;
        
        /// <value>Property <c>_rings</c> represents the rings of the player.</value>
        private int _rings;

        /// <value>Property <c>_isTimerOn</c> represents the timer status.</value>
        private bool _isTimerOn;
        
        /// <value>Property <c>_time</c> represents the time left.</value>
        private float _time;

        /// <value>Property <c>_pointsText</c> represents the UI element containing the points.</value>
        private TextMeshProUGUI _pointsText;
        
        /// <value>Property <c>_ringsText</c> represents the UI element containing the rings.</value>
        private TextMeshProUGUI _ringsText;
        
        /// <value>Property <c>_timeText</c> represents the UI element containing the time left.</value>
        private TextMeshProUGUI _timeText;
        
        /// <value>Property <c>_playerManager</c> represents the PlayerManager component of the player.</value>
        private PlayerManager _playerManager;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this) 
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad( this.gameObject );
        }

        /// <summary>
        /// Method <c>OnEnable</c> is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Method <c>OnSceneLoaded</c> is called when the scene is loaded.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _time = 10f;
            _isTimerOn = true;
            
            _pointsText = GameObject.Find("PointsText").GetComponent<TextMeshProUGUI>();
            _ringsText = GameObject.Find("RingsText").GetComponent<TextMeshProUGUI>();
            _timeText = GameObject.Find("TimeValueText").GetComponent<TextMeshProUGUI>();

            UpdatePointsText();
            UpdateRingsText();
            UpdateTimeText();
            
            _playerManager = GameObject.Find("Sonic").GetComponent<PlayerManager>();
        }

        /// <summary>
        /// Method <c>OnDisable</c> is called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// Method <c>FixedUpdate</c> is called every fixed frame-rate frame.
        /// </summary>
        private void FixedUpdate()
        {
            if (_isTimerOn)
            {
                if (_time >= 0)
                {
                    _time -= Time.deltaTime;
                    UpdateTimeText();
                }
                else
                {
                    _isTimerOn = false;
                    _playerManager.DieDirectly();
                }
            }
        }

        /// <summary>
        /// Method <c>LoseLife</c> makes the player loses a life and restarts the scene.
        /// </summary>
        public void LoseLife()
        {
            _lives--;
            if (_lives > 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else
                SceneManager.LoadScene("GameOver");
        }
        
        /// <summary>
        /// Method <c>AddPoints</c> adds points.
        /// </summary>
        public void AddPoints(int amount)
        {
            _points += amount;
            UpdatePointsText();
        }
        
        /// <summary>
        /// Method <c>UpdatePointsText</c> updates the points information on the UI.
        /// </summary>
        private void UpdatePointsText()
        {
            _pointsText.text = _points > 999999 ? "999999" : _points.ToString().PadLeft(6, '0');
        }
        
        /// <summary>
        /// Method <c>AddRings</c> adds rings.
        /// </summary>
        public void AddRings(int amount)
        {
            _rings += amount;
            UpdateRingsText();
        }
        
        /// <summary>
        /// Method <c>UpdateRingsText</c> updates the rings information on the UI.
        /// </summary>
        private void UpdateRingsText()
        {
            _ringsText.text = _rings > 99 ? "99" : _rings.ToString().PadLeft(2, '0');
        }
        
        /// <summary>
        /// Method <c>UpdateTimeText</c> updates the time information on the UI.
        /// </summary>
        private void UpdateTimeText()
        {
            var seconds = (int)_time;
            _timeText.text = seconds > 0 ? seconds.ToString().PadLeft(3, '0') : "000";
        }
        
        public void WinGame()
        {
            SceneManager.LoadScene("Win");
        }
        
        /// <summary>
        /// Method <c>RestartGame</c> restarts the game.
        /// </summary>
        public void RestartGame()
        {
            Destroy(gameObject);
            Instance = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        /// <summary>
        /// Method <c>QuitGame</c> quits the game.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
