using TMPro;
using UnityEngine;

namespace UI
{
    public class BulletsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private string _text = "Bullets ";
        
        public void UpdateBulletsView(int currentBullets, int maxBullets)
        {
            _healthText.text = _text + currentBullets + @"\" + maxBullets;
        }
    }
}