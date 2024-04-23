using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _hostGame;
        [SerializeField] private Button _joinGame;
        [SerializeField] private Button _changeSkin;
        [SerializeField] private Button _exitGame;
        
        [Space]
        [Header("SceneBuildIndex")]
        [SerializeField] private int _gameSceneBuildIndex = 1;

        [Space]
        [Header("Canvas")]
        [SerializeField] private Canvas _changeSkinCanvas;
        
        private void HostGame()
        {
            SceneManager.LoadScene(_gameSceneBuildIndex);
        }
        
        private void JoinGame()
        {
            SceneManager.LoadScene(_gameSceneBuildIndex);
        }

        private void EnterChangeSkin()
        {
            _changeSkinCanvas.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void OnEnable()
        {
            _hostGame.onClick.AddListener(HostGame);
            _joinGame.onClick.AddListener(JoinGame);
            _changeSkin.onClick.AddListener(EnterChangeSkin);
            _exitGame.onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            _hostGame.onClick.RemoveListener(HostGame);
            _joinGame.onClick.RemoveListener(JoinGame);
            _changeSkin.onClick.RemoveListener(EnterChangeSkin);
            _exitGame.onClick.RemoveListener(ExitGame);
        }
    }
}
