using Services;
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
            Animator.SetBool(Constants.ENEMY_DEATH, true);
        }

        public override void Exit()
        {
            Animator.SetBool(Constants.ENEMY_DEATH, false);
        }
    }
}