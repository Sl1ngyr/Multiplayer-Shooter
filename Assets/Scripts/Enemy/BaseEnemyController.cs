using System.Collections.Generic;
using Enemy.AnimationStates;
using Fusion;
using Services;
using UnityEngine;

namespace Enemy
{
    public abstract class BaseEnemyController : NetworkBehaviour
    {
        [SerializeField] protected EnemyData EnemyData;
        [SerializeField] protected float TimeToDespawn;
        
        [Networked] protected TickTimer AttackDelay { get; set; }
        [Networked] protected TickTimer DelayToDeath { get; set; }
        
        protected Rigidbody2D RigidbodyEnemy2D;
        protected Animator Animator;
        protected AnimationBehavior EnemyAnimationBehavior;
        protected List<Transform> ListTargetsToFollow;
        protected Transform TargetToFollow;
        protected EnemyCollisionDetector CollisionDetector;
        protected EnemyHealthSystem EnemyHealthSystem;

        protected bool IsReachTarget = false;
        protected bool IsEnemyDeath = false;

        public int EnemyDamage => EnemyData.Damage;
        public int EnemyHP => EnemyData.HP;

        public Transform TargetTransform
        {
            get => TargetToFollow;
            set => TargetToFollow = value;
        }
        
        public bool ReachTarget
        {
            get => IsReachTarget;
            set => IsReachTarget = value;
        }

        public bool IsEnemyDead => IsEnemyDeath;
        
        public void Init(List<Transform> targets)
        {
            ListTargetsToFollow = targets;
            Debug.Log(targets.Count.ToString());
            int randomTarget = Random.Range(0, targets.Count - 1);
            TargetToFollow = ListTargetsToFollow[randomTarget];
        }
        
        public override void Spawned()
        {
            EnemyHealthSystem = GetComponent<EnemyHealthSystem>();
            CollisionDetector = GetComponent<EnemyCollisionDetector>();
            RigidbodyEnemy2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            
            EnemyAnimationBehavior = new AnimationBehaviorEnemyRun(Animator);
            EnemyAnimationBehavior.Enter();

            CollisionDetector.OnEnemyActionsWhenTakeDamage += TakeDamageAnimation;
            EnemyHealthSystem.OnEnemyDeath += ActionsBeforeDie;
            
            AttackDelay = TickTimer.CreateFromSeconds(Runner, EnemyData.AttackDelay);
        }

        public override void FixedUpdateNetwork()
        {
            RPC_ChangeScale(TargetToFollow.transform.position.x);
            FollowToTarget();
        }
        
        protected void FollowToTarget()
        {
            if (IsEnemyDeath && DelayToDeath.ExpiredOrNotRunning(Runner))
            {
                EnemyDeath();
            }
            
            if(IsEnemyDeath) return;
            
            if (!IsReachTarget)
            {
                Vector3 direction = (TargetToFollow.transform.position - gameObject.transform.position).normalized;
                Vector3 positionToMove = transform.position + (EnemyData.Speed * Runner.DeltaTime * direction);
            
                RigidbodyEnemy2D.MovePosition(positionToMove);
            }
            else if (IsReachTarget && HasStateAuthority && AttackDelay.ExpiredOrNotRunning(Runner))
            {
                Attack();
                AttackDelay = TickTimer.CreateFromSeconds(Runner, EnemyData.AttackDelay);
            }
        }
        
        protected abstract void Attack();
        protected abstract void ActionsBeforeDie();

        public void SetNewTarget(Transform targetToRemove)
        {
            ListTargetsToFollow.Remove(targetToRemove);

            var randomTarget = Random.Range(0, ListTargetsToFollow.Count - 1);
            TargetToFollow = ListTargetsToFollow[randomTarget];
        }
        
        private void EnemyDeath()
        {
            Runner.Despawn(Object);
        }
        
        private void TakeDamageAnimation()
        {
            if(IsEnemyDeath) return;
            
            EnemyAnimationBehavior.Exit();
            EnemyAnimationBehavior = new AnimationBehaviorEnemyHit(Animator);
            EnemyAnimationBehavior.Enter();
        }
        
        [Rpc]
        private void RPC_ChangeScale(float directionX)
        {
            if (directionX > transform.position.x)
            {
                RigidbodyEnemy2D.transform.localScale = new Vector2(1, 1);
            }
            else if (directionX < transform.position.x)
            {
                RigidbodyEnemy2D.transform.localScale = new Vector2(-1, 1);
            }
        }
        
        private void OnDisable()
        { 
            CollisionDetector.OnEnemyActionsWhenTakeDamage -= TakeDamageAnimation;
            EnemyHealthSystem.OnEnemyDeath -= ActionsBeforeDie;
        }
        
    }
}