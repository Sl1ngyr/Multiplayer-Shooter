using TMPro;
using UnityEngine;

namespace UI
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private string _text = "HP ";
        
        public void UpdateHealthView(int currentHealth, int maxHealth)
        {
            _healthText.text = _text + currentHealth + @"\" + maxHealth;
        }
    }
}
