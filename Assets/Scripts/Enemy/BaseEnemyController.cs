using Enemy.AnimationStates;
using Fusion;
using Player.AnimationStates;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemyController : NetworkBehaviour
    {
        [SerializeField] protected EnemyData EnemyData;

        protected Rigidbody2D Rigidbody2D;
        protected Animator Animator;
        protected AnimationBehavior AnimationBehavior;
        protected Transform TargetToFollow;
        protected bool IsReachTarget;

        public bool ReachTarget
        {
            get => IsReachTarget;
            set => IsReachTarget = value;
        }
        
        public void Init(Transform target)
        {
            TargetToFollow = target;
        }
        
        public override void Spawned()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            AnimationBehavior = new AnimationBehaviorEnemyRun(Animator);
            AnimationBehavior.Enter();
        }
        
    }
}