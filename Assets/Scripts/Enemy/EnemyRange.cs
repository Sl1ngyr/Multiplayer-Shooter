using Enemy.AnimationStates;
using Fusion;
using Player;
using Services;
using UnityEngine;

namespace Enemy
{
    public class EnemyRange : BaseEnemyController
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _bulletDespawnDistance;
        
        private float _bulletRotation = -90;
        
        protected override void Attack()
        {
            Vector2 aimDirection = (TargetToFollow.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + _bulletRotation));
            
            Runner.Spawn(_bullet, _firePoint.transform.position, rotation, Object.InputAuthority, ((runner, o) =>
            {
                o.GetComponent<Bullet>().Init(EnemyData.Damage, _bulletDespawnDistance);
            }));
        }

        protected override void ActionsBeforeDie()
        {
            IsEnemyDeath = true;
            
            EnemyAnimationBehavior.Exit();
            EnemyAnimationBehavior = new AnimationBehaviorEnemyDeath(Animator);
            EnemyAnimationBehavior.Enter();

            RigidbodyEnemy2D.isKinematic = true;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            
            DelayToDeath = TickTimer.CreateFromSeconds(Runner, TimeToDespawn);
        }
    }
}