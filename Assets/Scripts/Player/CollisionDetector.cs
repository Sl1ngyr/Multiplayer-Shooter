﻿using Fusion;
using UnityEngine;
using System;
using Enemy;

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

            /*if (coll.transform.parent.TryGetComponent(out EnemyMelee enemyWeaponParent))
            {
                OnPlayerTakeDamage?.Invoke(enemyWeaponParent.EnemyDamage);
            }*/
        }
    }
}