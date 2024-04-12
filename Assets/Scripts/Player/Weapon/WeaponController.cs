using Fusion;
using Services.Network;
using UnityEngine;

namespace Player.Weapon
{
    public class WeaponController : NetworkBehaviour
    {
        [SerializeField] private GameObject _spawnPoint;
        
        [Networked] private TickTimer _shootDelay { get; set; }
        
        private Gun _currentWeapon;
        private WeaponData _weaponData;
        private int _numberOfBullets;
        private int _maxBullets;

        public void Init(WeaponData weaponData, NetworkObject gun)
        {
            _currentWeapon = gun.GetComponent<Gun>();
            _weaponData = weaponData;
            
            _maxBullets = _weaponData.NumberOfBullets;
            _numberOfBullets = _maxBullets;

            _shootDelay = TickTimer.CreateFromSeconds(Runner, _weaponData.ShootDelay);
            
            RPC_SetWeapon();
        }

        public override void FixedUpdateNetwork()
        {
            var input = GetInput(out NetworkInputData data);
            if (HasStateAuthority && _shootDelay.ExpiredOrNotRunning(Runner))
            {
                if (data.Aim.magnitude > 0)
                {
                    if (_numberOfBullets > 0)
                    {
                        --_numberOfBullets;
                        _currentWeapon.CreateBullet(transform, _weaponData.Damage, _weaponData.AttackDistance);
                        _shootDelay = TickTimer.CreateFromSeconds(Runner, _weaponData.ShootDelay);
                    }
                    
                    if (transform.localScale.x < 0)
                    {
                        _currentWeapon.RotateGun(-data.Aim);
                    }
                    else _currentWeapon.RotateGun(data.Aim);
                }
                
            }
        }

        public void RestoreAllBullets()
        {
            _numberOfBullets = _maxBullets;
        }
        
        [Rpc]
        private void RPC_SetWeapon()
        {
            _currentWeapon.transform.position = _spawnPoint.transform.position;
            _currentWeapon.transform.parent = _spawnPoint.transform;
        }
    }
}