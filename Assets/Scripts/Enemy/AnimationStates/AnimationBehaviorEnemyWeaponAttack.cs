using Player.AnimationStates;
using Services;
using UnityEngine;

namespace Enemy.AnimationStates
{
    public class AnimationBehaviorEnemyWeaponAttack : AnimationBehavior
    {
        public AnimationBehaviorEnemyWeaponAttack(Animator animator) : base(animator)
        {
        }

        public override void Enter()
        {
            Animator.SetBool(DescriptionEnemyAnimation.ENEMY_MELEE_WEAPON_ATTACK, true);
        }

        public override void Exit()
        {
            Animator.SetBool(DescriptionEnemyAnimation.ENEMY_MELEE_WEAPON_ATTACK, false);
        }
    }
}