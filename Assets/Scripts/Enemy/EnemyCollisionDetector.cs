using System;
using Fusion;
using Items;
using Player;
using Services;
using UnityEngine;

namespace Enemy
{
    public class EnemyCollisionDetector : NetworkBehaviour
    {
        public Action<int, int> OnEnemyTakeDamage;
        public Action OnEnemyActionsWhenTakeDamage;
        
        private void OnTriggerEnter2D(Collider2D coll)
        {
            
            if (coll.TryGetComponent(out Bullet enemyBullet))
            {
                if (enemyBullet.BulletOwner == BulletOwner.Player)
                {
                    OnEnemyTakeDamage?.Invoke(enemyBullet.BulletOwnerId,enemyBullet.Damage);
                    OnEnemyActionsWhenTakeDamage?.Invoke();
                }
            }
        }
        
        private void OnTriggerStay2D(Collider2D coll)
        {
            
            if (coll.TryGetComponent(out BombItem bomb))
            {
                if (bomb.IsBombDetonate)
                {
                    Runner.Despawn(Object);
                }
            }
        }
        
    }
}