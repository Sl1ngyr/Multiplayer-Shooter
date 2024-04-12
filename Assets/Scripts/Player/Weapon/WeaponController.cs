using Fusion;
using Services.Network;
using UI;
using UnityEngine;

namespace Player.Weapon
{
    public class WeaponController : NetworkBehaviour
    {
        [SerializeField] private GameObject _spawnPoint;
        
        [Networked] private TickTimer _shootDelay { get; set; }
        [Networked] private NetworkObject  _networkedBulletsView { get; set; }
        [Networked] private int  _networkedMaxBullets { get; set; }

        private Gun _currentWeapon;
        private BulletsView _bulletsView;
        private WeaponData _weaponData;
        private int _maxBullets;
        private int _numberOfBullets;
        
        public void Init(WeaponData weaponData, NetworkObject gun, NetworkObject bulletsView)
        {
            _currentWeapon = gun.GetComponent<Gun>();
            _weaponData = weaponData;

            _networkedMaxBullets = _weaponData.NumberOfBullets;
            
            _maxBullets = _weaponData.NumberOfBullets;
            _numberOfBullets = _weaponData.NumberOfBullets;
            
            _shootDelay = TickTimer.CreateFromSeconds(Runner, _weaponData.ShootDelay);
            
            _networkedBulletsView = bulletsView;

            RPC_SetWeapon();
        }

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                _bulletsView = _networkedBulletsView.GetComponent<BulletsView>();
                
                _bulletsView.UpdateBulletsView(_networkedMaxBullets,_networkedMaxBullets);
            }
        }

        [Rpc]
        private void RPC_SetBulletView(int currentBullets, int maxBullets)
        {
            if (Object.HasInputAuthority)
            {
                _bulletsView.UpdateBulletsView(currentBullets,maxBullets);
            }
            
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
                        RPC_SetBulletView(_numberOfBullets, _maxBullets);
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