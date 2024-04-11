using System;
using Services;

namespace Enemy
{
    public class EnemyHealthSystem : HealthSystem
    {
        private EnemyCollisionDetector _collisionDetector;
        private BaseEnemyController _enemyController;

        public Action OnEnemyDeath;
        
        public override void Spawned()
        {
            _enemyController = GetComponent<BaseEnemyController>();
            _collisionDetector = GetComponent<EnemyCollisionDetector>();
            
            _collisionDetector.OnEnemyTakeDamage += TakeDamage;

            MaxHealth = _enemyController.EnemyHP;
            
            CurrentHealth = MaxHealth;
        }
        
        protected override void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                OnEnemyDeath?.Invoke();
            }
        }
        
        private void OnDisable()
        { 
            _collisionDetector.OnEnemyTakeDamage -= TakeDamage;
        }
    }
}