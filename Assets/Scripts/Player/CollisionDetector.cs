using System;
using UnityEngine;
using Enemy;
using Fusion;

namespace Player
{
    public class CollisionDetector : NetworkBehaviour
    {
        public Action<int> OnPlayerTakeDamage;

        private void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.TryGetComponent(out Bullet enemyBullet))
            {
                if (enemyBullet.BulletOwner == BulletOwner.Enemy)
                {
                    RPC_EventTakeDamage(enemyBullet.Damage);
                }
            }
            
            if (coll.transform.parent != null)
            {
                if (coll.transform.parent.TryGetComponent(out EnemyMelee enemyWeaponParent))
                {
                    RPC_EventTakeDamage(enemyWeaponParent.EnemyDamage);
                }
                
            }
        }

        [Rpc]
        private void RPC_EventTakeDamage(int damage)
        {
            OnPlayerTakeDamage?.Invoke(damage);
        }
        
    }
    
}