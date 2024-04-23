using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _mainMenu;
        [SerializeField] private int _sceneBuildIndex = 0;
        
        private void OnEnable()
        {
            _mainMenu.onClick.AddListener(TransitionToMainMenu);
        }
        
        private void OnDisable()
        {
            _mainMenu.onClick.RemoveListener(TransitionToMainMenu);
        }

        private void TransitionToMainMenu()
        {
            SceneManager.LoadScene(_sceneBuildIndex);
        }
    }
}