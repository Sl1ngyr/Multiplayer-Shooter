using System;
using UnityEngine;
using Enemy;
using Fusion;

namespace Player
{
    public class CollisionDetector : NetworkBehaviour
    {
        public Action<int> OnPlayerTakeDamage;

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.TryGetComponent(out Bullet enemyBullet))
            {
                if (enemyBullet.BulletOwner == BulletOwner.Enemy)
                {
                    OnPlayerTakeDamage?.Invoke(enemyBullet.Damage);
                }
            }
            
            if (coll.transform.parent != null)
            {
                if (coll.transform.parent.TryGetComponent(out BaseEnemyController enemyWeaponParent))
                {
                    OnPlayerTakeDamage?.Invoke(enemyWeaponParent.EnemyDamage);
                }
                
            }
        }
        
    }
    
}