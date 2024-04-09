using Fusion;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyRange : BaseEnemyController
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _bulletDespawnDistance;
        
        [Networked] private TickTimer _shootDelay { get; set; }
        
        public override void FixedUpdateNetwork()
        {
            if (!IsReachTarget)
            {
                Vector3 direction = (TargetToFollow.transform.position - gameObject.transform.position).normalized;
                Vector3 positionToMove = transform.position + (EnemyData.Speed * Runner.DeltaTime * direction);
            
                Rigidbody2D.MovePosition(positionToMove);
            }
            else if (IsReachTarget&& HasStateAuthority && _shootDelay.ExpiredOrNotRunning(Runner))
            {
                _shootDelay = TickTimer.CreateFromSeconds(Runner, EnemyData.AttackDelay);
                Attack();
            }
        }
        
        private void Attack()
        {
            Vector2 aimDirection = (TargetToFollow.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Runner.Spawn(_bullet, _firePoint.transform.position, rotation, Object.InputAuthority, ((runner, o) =>
            {
                o.GetComponent<Bullet>().Init(EnemyData.Damage, _bulletDespawnDistance, transform.localScale.x);
            }));
        }
        
    }
}