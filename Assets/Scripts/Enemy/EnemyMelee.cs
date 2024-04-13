using System;
using Enemy.AnimationStates;
using Fusion;
using UnityEngine;

namespace Enemy
{
    public class EnemyMelee : BaseEnemyController
    {
        public Action OnAttacked;
        public Action OnEnemyMeleeDead;
        
        protected override void Attack()
        {
            OnAttacked?.Invoke();
        }

        protected override void ActionsBeforeDie()
        {
            if(IsEnemyDeath) return;
            IsEnemyDeath = true;
            
            EnemyAnimationBehavior.Exit();
            EnemyAnimationBehavior = new AnimationBehaviorEnemyDeath(Animator);
            EnemyAnimationBehavior.Enter();
            
            RigidbodyEnemy2D.isKinematic = true;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

            RPC_DeactivateWeapon();
            
            DelayToDeath = TickTimer.CreateFromSeconds(Runner, TimeToDespawn);
        }
        
        [Rpc]
        private void RPC_DeactivateWeapon()
        {
            OnEnemyMeleeDead?.Invoke();
        }
    }
}