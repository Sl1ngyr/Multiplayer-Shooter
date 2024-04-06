using System.Collections.Generic;
using UnityEditor;
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
        
        [SerializeField] private SpriteRenderer _prefab;
        
        private void SelectSkin()
        {
            var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            var image = button.GetComponent<Image>();
            _prefab.sprite = image.sprite;
        }

        private void SaveSkin()
        {
            PrefabUtility.SaveAsPrefabAsset(_prefab.gameObject, "Assets/Resources/Prefabs/Player.prefab");
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
