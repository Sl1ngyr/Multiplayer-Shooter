using Fusion;
using UnityEngine;

namespace Player
{
    public class Gun : NetworkBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform firePoint;

        public void RotateGun(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - transform.rotation.y;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
        }
        
        public void CreateBullet(Transform _playerTransform, float damage, float distance)
        {
            Runner.Spawn(_bulletPrefab, firePoint.transform.position, transform.rotation, Object.InputAuthority, ((runner, o) =>
            {
                o.GetComponent<Bullet>().Init(damage, distance, _playerTransform.localScale.x);
            }));
        }
    }
}