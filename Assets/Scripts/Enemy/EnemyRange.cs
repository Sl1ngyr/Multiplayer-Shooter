using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyRange : BaseEnemyController
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _bulletDespawnDistance;
        
        public override void FixedUpdateNetwork()
        {
            FollowToTarget();
        }
        
        protected override void Attack()
        {
            Vector2 aimDirection = (TargetToFollow.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle)) ;
            
            Runner.Spawn(_bullet, _firePoint.transform.position, rotation, Object.InputAuthority, ((runner, o) =>
            {
                o.GetComponent<Bullet>().Init(EnemyData.Damage, _bulletDespawnDistance, 1);
            }));
        }
        
    }
}