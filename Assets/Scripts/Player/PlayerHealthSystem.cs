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

        public override void Spawned()
        {
            CurrentHealth = MaxHealth;
            
            _collisionDetector = GetComponent<CollisionDetector>();

            _collisionDetector.OnPlayerTakeDamage += TakeDamage;

            if (Object.HasInputAuthority)
            {
                _healthView = FindAnyObjectByType<HealthView>(FindObjectsInactive.Include); 
                _healthView.gameObject.SetActive(true);
                _healthView.UpdateHealthView(CurrentHealth, MaxHealth);
            }
        }
        
        public void RestoreHealth()
        {
            CurrentHealth = MaxHealth;
            _healthView.UpdateHealthView(CurrentHealth, MaxHealth);
        }
        
        protected override void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (Object.HasInputAuthority)
            {
                _healthView.UpdateHealthView(CurrentHealth, MaxHealth);
            }

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