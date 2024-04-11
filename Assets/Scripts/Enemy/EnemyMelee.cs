using Enemy.AnimationStates;
using Fusion;
using UnityEngine;

namespace Enemy
{
    public class EnemyMelee : BaseEnemyController
    {
        [SerializeField] private Animator _weaponAnimator;

        protected override void Attack()
        {
            _weaponAnimator.SetTrigger(DescriptionEnemyAnimation.ENEMY_MELEE_WEAPON_ATTACK);
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

            _weaponAnimator.transform.gameObject.SetActive(false);
            
            DelayToDeath = TickTimer.CreateFromSeconds(Runner, TimeToDespawn);
        }
    }
}