using Player.AnimationStates;
using UnityEngine;

namespace Enemy.AnimationStates
{
    public class AnimationBehaviorEnemyDeath : AnimationBehavior
    {
        public AnimationBehaviorEnemyDeath(Animator animator) : base(animator)
        {
        }

        public override void Enter()
        {
            Animator.SetBool(DescriptionEnemyAnimation.ENEMY_DEATH, true);
        }

        public override void Exit()
        {
            Animator.SetBool(DescriptionEnemyAnimation.ENEMY_DEATH, false);
        }
    }
}