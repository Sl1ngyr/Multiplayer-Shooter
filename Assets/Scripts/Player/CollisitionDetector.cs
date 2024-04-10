using Fusion;
using UnityEngine;
using System;
using Enemy;

namespace Player
{
    public class CollisitionDetector : NetworkBehaviour
    {
        public Action<int> OnTakeDamage;

        private void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.TryGetComponent(out Bullet enemyBullet))
            {
                Bullet bullet = enemyBullet;
                if (bullet._BulletOwner == BulletOwner.Enemy)
                {
                    OnTakeDamage?.Invoke(bullet.Damage);
                    
                }
            }

            if (coll.transform.parent.TryGetComponent(out BaseEnemyController enemyWeaponParent))
            {
                BaseEnemyController enemy = enemyWeaponParent;
                
                OnTakeDamage?.Invoke(enemy.EnemyDamage);
                Debug.Log("enemyDamage");
            }
        }
    }
}