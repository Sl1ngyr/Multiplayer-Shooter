using Enemy.AnimationStates;
using UnityEngine;

namespace Enemy
{
    public class EnemyMelee : BaseEnemyController
    {
        [SerializeField] private Animator _weaponAnimator;
        
        public override void FixedUpdateNetwork()
        {
            FollowToTarget();
        }
        
        protected override void Attack()
        {
            _weaponAnimator.SetTrigger(DescriptionEnemyAnimation.ENEMY_MELEE_WEAPON_ATTACK);
        }
    }
}