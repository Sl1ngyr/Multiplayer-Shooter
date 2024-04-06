using UnityEngine;

namespace Player
{
    public class SkinHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _selectedSkin;

        private void Awake()
        {
            PutOnSelectedSkin();
        }

        private void PutOnSelectedSkin()
        {
            _spriteRenderer.sprite = _selectedSkin.sprite;
        }
    }
}