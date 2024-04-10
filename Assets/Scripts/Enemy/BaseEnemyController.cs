using Enemy.AnimationStates;
using Fusion;
using Player.AnimationStates;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemyController : NetworkBehaviour
    {
        [SerializeField] protected EnemyData EnemyData;
        
        [Networked] protected TickTimer AttackDelay { get; set; }
        
        protected Rigidbody2D Rigidbody2D;
        protected Animator Animator;
        protected AnimationBehavior EnemyAnimationBehavior;
        protected Transform TargetToFollow;
        protected bool IsReachTarget = false;

        protected abstract void Attack();
        
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
            EnemyAnimationBehavior = new AnimationBehaviorEnemyRun(Animator);
            EnemyAnimationBehavior.Enter();
        }

        protected void FollowToTarget()
        {
            RPC_ChangeScale(TargetToFollow.transform.position.x);
            
            if (!IsReachTarget)
            {
                EnemyAnimationBehavior.Exit();
                EnemyAnimationBehavior = new AnimationBehaviorEnemyRun(Animator);
                EnemyAnimationBehavior.Enter();
                
                Vector3 direction = (TargetToFollow.transform.position - gameObject.transform.position).normalized;
                Vector3 positionToMove = transform.position + (EnemyData.Speed * Runner.DeltaTime * direction);
            
                Rigidbody2D.MovePosition(positionToMove);
            }
            else if (IsReachTarget && HasStateAuthority && AttackDelay.ExpiredOrNotRunning(Runner))
            {
                Attack();
                AttackDelay = TickTimer.CreateFromSeconds(Runner, EnemyData.AttackDelay);
            }
        }

        [Rpc]
        protected void RPC_ChangeScale(float directionX)
        {
            if (directionX > transform.position.x)
            {
                Rigidbody2D.transform.localScale = new Vector2(1, 1);
            }
            else if (directionX < transform.position.x)
            {
                Rigidbody2D.transform.localScale = new Vector2(-1, 1);
            }
        }
        
    }
}