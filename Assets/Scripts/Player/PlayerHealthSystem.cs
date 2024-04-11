using Services;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerHealthSystem : HealthSystem
    {
        [SerializeField] private AnimationController _animationController;
        
        private HealthView _healthView;
        private CollisionDetector _collisionDetector;
        
        public void Init(HealthView healthView)
        {
            _healthView = healthView;
            _healthView.UpdateHealthView(CurrentHealth, MaxHealth);
        }
        
        public override void Spawned()
        {
            _collisionDetector = GetComponent<CollisionDetector>();
            
            _collisionDetector.OnPlayerTakeDamage += TakeDamage;
            
            CurrentHealth = MaxHealth;
        }

        protected override void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            
            _healthView.UpdateHealthView(CurrentHealth, MaxHealth);
            
            if (CurrentHealth <= 0)
            {
                _animationController.PlayerDeath();
            }
        }
        
        private void OnDisable()
        { 
            _collisionDetector.OnPlayerTakeDamage -= TakeDamage;
        }
    }
}