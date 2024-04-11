using System;
using Fusion;
using Player;
using UnityEngine;
namespace Enemy
{
    public class EnemyCollisionDetector : NetworkBehaviour
    {
        public Action<int> OnEnemyTakeDamage;
        public Action OnEnemyActionsWhenTakeDamage;
        
        private void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.TryGetComponent(out Bullet enemyBullet))
            {
                Bullet bullet = enemyBullet;
                if (bullet.BulletOwner == BulletOwner.Player)
                {
                    OnEnemyTakeDamage?.Invoke(bullet.Damage);
                    OnEnemyActionsWhenTakeDamage?.Invoke();
                }
            }
            
        }
    }
}