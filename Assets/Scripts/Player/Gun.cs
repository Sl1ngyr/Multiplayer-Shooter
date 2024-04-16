using Fusion;
using Player.Weapon;
using Services;
using UnityEngine;

namespace Player
{
    public class Gun : NetworkBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float _spreadAngle = 10f;
        [SerializeField] private int _countBulletsForShotgun = 3;

        private float _bulletRotation = -90;
        
        public void RotateGun(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - transform.rotation.y;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
        }
        
        public void CreateBullet(Transform playerTransform, int damage, float distance, ShotType shotType)
        {
            switch (shotType)
            {
                case ShotType.Single:
                    SingleShot(playerTransform, damage, distance);
                    break;
                case ShotType.Shotgun:
                    ShotgunShot(playerTransform, damage, distance);
                    break;
            }
        }
        
        private void SingleShot(Transform playerTransform, int damage, float distance)
        {
            Quaternion rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z + _bulletRotation * playerTransform.localScale.x);
            
            Runner.Spawn(_bulletPrefab, firePoint.transform.position, rotation, Object.InputAuthority, ((runner, o) =>
            {
                o.GetComponent<Bullet>().Init(damage, distance);
            }));
        }

        private void ShotgunShot(Transform playerTransform, int damage, float distance)
        {
            float angleStep = _spreadAngle / _countBulletsForShotgun;
            float aimAngle = transform.rotation.eulerAngles.z + _bulletRotation * playerTransform.localScale.x;

            for (int i = 0; i < _countBulletsForShotgun; i++)
            {
                float currentBulletAngle = angleStep * i;
                Quaternion rotation = Quaternion.Euler(new Vector3(0,0,aimAngle + currentBulletAngle));
                
                Runner.Spawn(_bulletPrefab, firePoint.transform.position, rotation, Object.InputAuthority, ((runner, o) =>
                {
                    o.GetComponent<Bullet>().Init(damage, distance);
                }));
            }
        }
    }
}