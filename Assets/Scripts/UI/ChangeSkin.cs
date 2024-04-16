using System.Collections.Generic;
using Services;
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
        
        private void SelectSkin()
        {
            var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            var skinName = button.GetComponent<ButtonSkinsDescription>().SkinName;
            PlayerPrefs.SetString(Constants.PLAYER_PREFS_SKIN, skinName);
            
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
