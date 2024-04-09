using Fusion;
using UnityEngine;

namespace Player
{
    public class SkinHandler : NetworkBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _selectedSkin;

        public override void Spawned()
        {
            PutOnSelectedSkin();
        }

        private void PutOnSelectedSkin()
        {
            _spriteRenderer.sprite = _selectedSkin.sprite;
        }
    }
}