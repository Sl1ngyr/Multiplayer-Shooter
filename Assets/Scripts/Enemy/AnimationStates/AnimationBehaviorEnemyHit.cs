using Player.AnimationStates;
using UnityEngine;

namespace Enemy.AnimationStates
{
    public class AnimationBehaviorEnemyHit : AnimationBehavior
    {
        public AnimationBehaviorEnemyHit(Animator animator) : base(animator)
        {
        }

        public override void Enter()
        {
            Animator.SetTrigger(DescriptionEnemyAnimation.ENEMY_HIT);
        }

        public override void Exit()
        {
            Animator.ResetTrigger(DescriptionEnemyAnimation.ENEMY_HIT); 
        }
    }
}