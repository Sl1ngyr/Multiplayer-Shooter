using Fusion;
using Services;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerHealthSystem : HealthSystem
    {
        [SerializeField] private AnimationController _animationController;
        
        [Networked] private NetworkObject _networkHealthView { get; set; }

        private HealthView _healthView;
        private CollisionDetector _collisionDetector;
        
        public void Init(NetworkObject networkObject)
        {
            _networkHealthView = networkObject;
        }
        
        public override void Spawned()
        {
            CurrentHealth = MaxHealth;
            
            _collisionDetector = GetComponent<CollisionDetector>();

            _collisionDetector.OnPlayerTakeDamage += TakeDamage;

            if (Object.HasInputAuthority)
            {
                _healthView = _networkHealthView.GetComponent<HealthView>();
                
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