using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChangeSkin : MonoBehaviour
    {
        [SerializeField] private List<Button> _skinsButtons;

        [SerializeField] private Button _saveSkin;
        [SerializeField] private Button _closeSkinMenu;

        [SerializeField] private Canvas _startMenu;
        
        public const string PLAYER_PREFS_SKIN = "Skin";
        
        private void SelectSkin()
        {
            var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            var skinName = button.GetComponent<ButtonSkinsDescription>().SkinName;
            Debug.Log(skinName + "Selected skin");
            PlayerPrefs.SetString(PLAYER_PREFS_SKIN, skinName);
            Debug.Log(PlayerPrefs.GetString(ChangeSkin.PLAYER_PREFS_SKIN) + "NOW SKin");
        }

        private void SaveSkin()
        {
            CloseSkinMenu();
        }

        private void CloseSkinMenu()
        {
            _startMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            foreach (var button in _skinsButtons)
            {
                button.onClick.AddListener(SelectSkin);
            }
            
            _saveSkin.onClick.AddListener(SaveSkin);
            _closeSkinMenu.onClick.AddListener(CloseSkinMenu);
        }

        private void OnDisable()
        {
            foreach (var button in _skinsButtons)
            {
                button.onClick.RemoveListener(SelectSkin);
            }
            
            _saveSkin.onClick.RemoveListener(SaveSkin);
            _closeSkinMenu.onClick.RemoveListener(SaveSkin);
        }
        
    }
}
