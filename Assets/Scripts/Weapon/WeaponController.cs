using Fusion;
using Player;
using Services;
using UnityEngine;


namespace Weapon
{
    public class WeaponController : NetworkBehaviour
    {
        [SerializeField] private GameObject _spawnPoint;
        
        private Gun _currentWeapon;
        private string nameWeapon;
        [Networked] private TickTimer _shootDelay { get; set; }
        
        private WeaponData _weaponData;
        
        public void InitWeaponData(WeaponData weaponData, NetworkObject gun)
        {
            _currentWeapon = gun.GetComponent<Gun>();
            _weaponData = weaponData;
            RPC_SetWeapon();
        }

        [Rpc]
        private void RPC_SetWeapon()
        {
            _currentWeapon.transform.position = _spawnPoint.transform.position;
            _currentWeapon.transform.parent = _spawnPoint.transform;
            
        }
        
        public override void FixedUpdateNetwork()
        {
            var input = GetInput(out NetworkInputData data);
            if (HasStateAuthority && _shootDelay.ExpiredOrNotRunning(Runner))
            {
                if (data.Aim.magnitude > 0)
                {
                    _currentWeapon.CreateBullet(transform, _weaponData.Damage, _weaponData.AttackDistance);
                    _shootDelay = TickTimer.CreateFromSeconds(Runner, _weaponData.ShootDelay);
                    if (transform.localScale.x < 0)
                    {
                        _currentWeapon.RotateGun(-data.Aim);
                    }
                    else _currentWeapon.RotateGun(data.Aim);
                    
                }
                
            }
        }

    }
}