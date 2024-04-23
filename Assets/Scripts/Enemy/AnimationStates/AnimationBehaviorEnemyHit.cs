using Services;
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
            Animator.SetTrigger(Constants.ENEMY_HIT);
        }

        public override void Exit()
        {
            Animator.ResetTrigger(Constants.ENEMY_HIT); 
        }
    }
}