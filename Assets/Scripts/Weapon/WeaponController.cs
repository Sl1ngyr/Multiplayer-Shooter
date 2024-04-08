using System;
using Fusion;
using Player;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Weapon
{
    public class WeaponController : NetworkBehaviour
    {
        [SerializeField] private Gun _weaponPrefab;

        private Gun _currentWeapon;
        private string nameWeapon;
        [Networked] private TickTimer _shootDelay { get; set; }
        
        private WeaponData _weaponData;
        
        public void InitWeaponData(WeaponData weaponData)
        {
            _weaponData = weaponData;
            nameWeapon = _weaponData.Sprite.name;
            //RPC_InitWeapon(nameWeapon);
        }

        private void Start()
        {
            InitWeapon();
        }
        
        private void InitWeapon()
        {
            _currentWeapon = Runner.Spawn(_weaponPrefab, transform.position, Quaternion.identity, Object.InputAuthority);
            _currentWeapon.transform.parent = transform;
            _currentWeapon.Init(Resources.Load<Sprite>("Sprites/" + nameWeapon));
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