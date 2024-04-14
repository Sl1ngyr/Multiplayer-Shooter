using Fusion;
using Services;
using UI;
using System;

namespace Player
{
    public class PlayerHealthSystem : HealthSystem
    {

        [Networked] private NetworkObject _networkHealthView { get; set; }

        private HealthView _healthView;
        private CollisionDetector _collisionDetector;

        private bool _isPlayerDead = false;
        
        public Action OnPlayerDead;
        public Action<int> OnPlayerLoseLifeEvent;
        
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
                _healthView.gameObject.SetActive(true);
                _healthView.UpdateHealthView(CurrentHealth, MaxHealth);
            }
        }
        
        public void RestoreHealth()
        {
            CurrentHealth = MaxHealth;
            
            RPC_SetHealthView(CurrentHealth, MaxHealth);
        }
        
        protected override void TakeDamage(int damage)
        {
            if(_isPlayerDead) return;

            CurrentHealth -= damage;

            RPC_SetHealthView(CurrentHealth, MaxHealth);

            if (CurrentHealth <= 0)
            {
                OnPlayerDead?.Invoke();
                OnPlayerLoseLifeEvent?.Invoke(Object.InputAuthority.PlayerId);
                
                _isPlayerDead = true;
                
                RPC_ManagementStatusHealthView(false);
            }
        }

        [Rpc]
        private void RPC_ManagementStatusHealthView(bool status)
        {
            if (Object.HasInputAuthority)
            {
                _healthView.gameObject.SetActive(status);
            }
        }
        
        [Rpc]
        private void RPC_SetHealthView(int currentHealth, int maxHealth)
        {
            if (Object.HasInputAuthority)
            {
                _healthView.UpdateHealthView(currentHealth, maxHealth);
            }
            
        }
        
        private void OnDisable()
        { 
            _collisionDetector.OnPlayerTakeDamage -= TakeDamage;
        }
    }
}