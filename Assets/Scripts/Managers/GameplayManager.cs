using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        
        /// <value>Property <c>_isGameOver</c> defines if the game is over.</value>
        private bool _isGameOver;
        
        /// <value>Property <c>_isWin</c> defines if the game is won.</value>
        private bool _isWin;

        /// <value>Property <c>_pointsText</c> represents the UI element containing the points.</value>
        private TextMeshProUGUI _pointsText;
        
        /// <value>Property <c>_ringsText</c> represents the UI element containing the rings.</value>
        private TextMeshProUGUI _ringsText;
        
        /// <value>Property <c>_timeText</c> represents the UI element containing the time left.</value>
        private TextMeshProUGUI _timeText;
        
        /// <value>Property <c>_infoTitleText</c> represents the UI element containing the info title.</value>
        private TextMeshProUGUI _infoTitleText;
        
        /// <value>Property <c>_infoLivesText</c> represents the UI element containing the info lives.</value>
        private TextMeshProUGUI _infoLivesText;
        
        /// <value>Property <c>_playerManager</c> represents the PlayerManager component of the player.</value>
        private PlayerManager _playerManager;
        
        /// <value>Property <c>_cameraAudioSource</c> represents the AudioSource component of the camera.</value>
        private AudioSource _cameraAudioSource;
        
        /// <value>Property <c>AudioClips</c> represents a dictionary containing all sounds and music for the game.</value>
        public Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();

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
            
            AudioClips.Add("mainTheme", Resources.Load<AudioClip>("Music/smb_above_ground"));
            
            AudioClips.Add("jumpSound", Resources.Load<AudioClip>("Sounds/smb_jump-small"));
            AudioClips.Add("growSound", Resources.Load<AudioClip>("Sounds/smb_powerup"));
            AudioClips.Add("shrinkSound", Resources.Load<AudioClip>("Sounds/smb_pipe"));
            AudioClips.Add("diePlayerSound", Resources.Load<AudioClip>("Sounds/smb_mariodie"));
            AudioClips.Add("dieEnemySound", Resources.Load<AudioClip>("Sounds/smb_stomp"));
            
            AudioClips.Add("powerUpSound", Resources.Load<AudioClip>("Sounds/smb_powerup_appears"));
            AudioClips.Add("extraLifeSound", Resources.Load<AudioClip>("Sounds/smb_1-up"));
            AudioClips.Add("coinSound", Resources.Load<AudioClip>("Sounds/smb_coin"));
            
            AudioClips.Add("blockBreakSound", Resources.Load<AudioClip>("Sounds/smb_breakblock"));
            AudioClips.Add("blockBounceSound", Resources.Load<AudioClip>("Sounds/smb_bump"));
            
            AudioClips.Add("gameWinSound", Resources.Load<AudioClip>("Sounds/smb_stage_clear"));
            AudioClips.Add("gameOverSound", Resources.Load<AudioClip>("Sounds/smb_gameover"));
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
            _pointsText = GameObject.Find("PointsText").GetComponent<TextMeshProUGUI>();
            _ringsText = GameObject.Find("RingsText").GetComponent<TextMeshProUGUI>();
            _timeText = GameObject.Find("TimeValueText").GetComponent<TextMeshProUGUI>();

            UpdatePointsText();
            UpdateRingsText();

            if (scene.name == "Game")
            {
                _time = 300f;
                _isTimerOn = true;
                UpdateTimeText();
                
                _playerManager = GameObject.Find("Sonic").GetComponent<PlayerManager>();
                
                _cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
                _cameraAudioSource.clip = AudioClips.TryGetValue("mainTheme", out AudioClip clip) ? clip : null;
                _cameraAudioSource.Play();
            }
            else if (scene.name == "Info")
            {
                _isTimerOn = false;
                _timeText.text = String.Empty;
                
                _infoTitleText = GameObject.Find("TitleText").GetComponent<TextMeshProUGUI>();
                _infoLivesText = GameObject.Find("LivesText").GetComponent<TextMeshProUGUI>();

                if (_isWin)
                {
                    _infoTitleText.text = "You Win!";
                    _infoLivesText.text = String.Empty;
                }
                else if (_isGameOver)
                {
                    _infoTitleText.text = "Game Over";
                    _infoLivesText.text = String.Empty;
                    
                    _cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
                    _cameraAudioSource.PlayOneShot(AudioClips.TryGetValue("gameOverSound", out AudioClip clip) ? clip : null);
                }
                else
                {
                    _infoTitleText.text = "Sonic";
                    _infoLivesText.text = "x " + _lives.ToString();
                    StartCoroutine(StartGame());
                }
            }
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
        /// Method <c>AddLives</c> adds points.
        /// </summary>
        public void AddLives(int amount)
        {
            _cameraAudioSource.PlayOneShot(AudioClips.TryGetValue("extraLifeSound", out AudioClip clip) ? clip : null);
            _lives += amount;
        }

        /// <summary>
        /// Method <c>LoseLife</c> makes the player loses a life and restarts the scene.
        /// </summary>
        public void LoseLife()
        {
            _lives--;
            if (_lives <= 0)
                _isGameOver = true;
            SceneManager.LoadScene("Info");
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
        
        /// <summary>
        /// Method <c>WinGame</c> wins the game.
        /// </summary>
        public void WinGame()
        {
            _isWin = true;
            SceneManager.LoadScene("Info");
        }

        /// <summary>
        /// Method <c>StartGame</c> starts the game.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Game");
        }
        
        
        /// <summary>
        /// Method <c>RestartGame</c> restarts the game.
        /// </summary>
        public void RestartGame()
        {
            Destroy(gameObject);
            Instance = null;
            SceneManager.LoadScene("Info");
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
