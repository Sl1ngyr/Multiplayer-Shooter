using Fusion;
using Player.AnimationStates;
using UI;
using UnityEngine;

namespace Player
{
    public class HealthSystem : NetworkBehaviour
    {
        [SerializeField] private AnimationController _animationController;
        [SerializeField] private int _maxHealth;
        
        private HealthView _healthView;
        private CollisitionDetector _collisitionDetector;
        private int _currentHealth;
        
        public override void Spawned()
        {
            _collisitionDetector = GetComponent<CollisitionDetector>();
            
            _collisitionDetector.OnTakeDamage += TakeDamage;
            
            _currentHealth = _maxHealth;
        }

        public void Init(HealthView healthView)
        {
            _healthView = healthView;
            _healthView.UpdateHealthView(_currentHealth, _maxHealth);
        }
        
        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            
            _healthView.UpdateHealthView(_currentHealth, _maxHealth);
            
            if (_currentHealth <= 0)
            {
                _animationController.PlayerDeath();
            }
        }

        private void OnDisable()
        { 
            _collisitionDetector.OnTakeDamage -= TakeDamage;
        }
    }
}