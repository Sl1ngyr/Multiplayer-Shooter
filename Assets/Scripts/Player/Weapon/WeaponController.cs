﻿using Fusion;
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
        private PlayerHealthSystem _healthSystem;
        
        private int _maxBullets;
        private int _numberOfBullets;
        private bool _isPlayerAlive = true;
        
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
                _bulletsView.gameObject.SetActive(true);
                _bulletsView.UpdateBulletsView(_networkedMaxBullets,_networkedMaxBullets);
            }

            _healthSystem = GetComponent<PlayerHealthSystem>();
            _healthSystem.OnPlayerDead += DeactivateComponents;
        }
        
        public override void FixedUpdateNetwork()
        {
            if(!_isPlayerAlive) return;
            
            var input = GetInput(out NetworkInputData data);
            
            if (HasStateAuthority)
            {
                if (data.Aim.magnitude > 0)
                {
                    CheckLocalScaleForRotationGun(data.Aim);
                    
                    if (_shootDelay.ExpiredOrNotRunning(Runner) && _numberOfBullets > 0)
                    {
                        --_numberOfBullets;
                        RPC_SetBulletView(_numberOfBullets, _maxBullets);
                        
                        _currentWeapon.CreateBullet(transform, _weaponData.Damage, _weaponData.AttackDistance, _weaponData.ShootTypeWeapon);
                        _shootDelay = TickTimer.CreateFromSeconds(Runner, _weaponData.ShootDelay);
                    }
                }
            }
        }

        private void CheckLocalScaleForRotationGun(Vector2 aim)
        {
            if (transform.localScale.x < 0)
            {
                _currentWeapon.RotateGun(-aim);
            }
            else
            {
                _currentWeapon.RotateGun(aim);
            }
        }
        
        public void RestoreAllBullets()
        {
            _numberOfBullets = _maxBullets;
            RPC_SetBulletView(_numberOfBullets, _maxBullets);
        }

        private void DeactivateComponents()
        {
            Runner.Despawn(_currentWeapon.Object);

            RPC_ManagementStatusBulletView(false);

            _isPlayerAlive = false;
        }
        
        [Rpc]
        private void RPC_SetBulletView(int currentBullets, int maxBullets)
        {
            if (Object.HasInputAuthority)
            {
                _bulletsView.UpdateBulletsView(currentBullets,maxBullets);
            }
            
        }
        
        [Rpc]
        private void RPC_ManagementStatusBulletView(bool status)
        {
            if (Object.HasInputAuthority)
            {
                _bulletsView.gameObject.SetActive(status);
            }
        }
        
        [Rpc]
        private void RPC_SetWeapon()
        {
            _currentWeapon.transform.position = _spawnPoint.transform.position;
            _currentWeapon.transform.parent = _spawnPoint.transform;
        }

        private void OnDisable()
        {
            _healthSystem.OnPlayerDead -= DeactivateComponents;
        }
    }
}