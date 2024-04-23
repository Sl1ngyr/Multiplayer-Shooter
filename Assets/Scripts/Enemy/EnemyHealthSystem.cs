using System;
using Services;

namespace Enemy
{
    public class EnemyHealthSystem : HealthSystem
    {
        private EnemyCollisionDetector _collisionDetector;
        private BaseEnemyController _enemyController;
        private EnemySpawner _enemySpawner;
        
        public Action OnEnemyDeath;

        public void Init(EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
        }
        
        public override void Spawned()
        {
            _enemyController = GetComponent<BaseEnemyController>();
            _collisionDetector = GetComponent<EnemyCollisionDetector>();
            
            _collisionDetector.OnEnemyTakeDamage += TakeDamage;

            MaxHealth = _enemyController.EnemyHP;
            
            CurrentHealth = MaxHealth;
        }

        protected override void TakeDamage(int id, int damage)
        {
            if(_enemyController.IsEnemyDead) return;
            
            int currentHealth = CurrentHealth;
            
            CurrentHealth -= damage;
            
            if (CurrentHealth <= 0)
            {
                _enemySpawner.RecordDamageFromPlayer(id, currentHealth);
                _enemySpawner.RecordKillFromPlayer(id);

                OnEnemyDeath?.Invoke();
            }
            else
            {
                _enemySpawner.RecordDamageFromPlayer(id, damage);
            }
            
        }
        
        private void OnDisable()
        { 
            _collisionDetector.OnEnemyTakeDamage -= TakeDamage;
        }
    }
}