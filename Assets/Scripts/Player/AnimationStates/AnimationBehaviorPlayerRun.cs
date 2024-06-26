﻿using Services;
using UnityEngine;

namespace Player.AnimationStates
{
    public class AnimationBehaviorPlayerRun : AnimationBehavior
    {
        public AnimationBehaviorPlayerRun(Animator animator) : base(animator)
        {
        }

        public override void Enter()
        {
            Animator.SetBool(Constants.PLAYER_RUN, true);
        }

        public override void Exit()
        {
            Animator.SetBool(Constants.PLAYER_RUN, false);
        }
    }
}