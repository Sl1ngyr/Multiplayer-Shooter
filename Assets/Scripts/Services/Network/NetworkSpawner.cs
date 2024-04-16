using System.Collections.Generic;
using Fusion;
using Player;
using UnityEngine;
using Player.Weapon;
using Wave;

namespace Services.Network
{
    public class NetworkSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] private NetworkRunner _networkRunner;
        
        [SerializeField] private List<NetworkObject> _playerPrefab;
        [SerializeField] private List<WeaponData> _weaponDatas;
        [SerializeField] private List<NetworkObject> _playerGuns;
        
        [SerializeField] private NetworkObject _bulletsView;
        [SerializeField] private NetworkObject _healthView;
        [SerializeField] private NetworkObject _joystiksView;
        
        [SerializeField] private StatisticsPlayersData _statisticsPlayers;
        [SerializeField] private WaveController _waveController;
        
        [Networked] private string _playerPrefsSkin { get; set; }
        private ChangeDetector _changeDetector;
        private PlayerRef _player;

        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        private Dictionary<PlayerRef, NetworkObject> _spawnedWeapons = new Dictionary<PlayerRef, NetworkObject>();
        
        private int _maxPlayers = 2;

        public override void Spawned()
        {
            _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
            
            RPC_ChangeSkin(PlayerPrefs.GetString(Constants.PLAYER_PREFS_SKIN));
        }
        
        public override void Render()
        {
            foreach (var change in _changeDetector.DetectChanges(this))
            {
                switch (change)
                {
                    case nameof(_playerPrefsSkin):
                        if (_playerPrefsSkin == "") break;
                        
                        SpawnHandler(_player);
                        _playerPrefsSkin = "";
                        break;
                }
            }
        }
        
        public void PlayerJoined(PlayerRef player)
        {
            _player = player;
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject playerObject))
            {
                _networkRunner.Despawn(playerObject);
                _spawnedCharacters.Remove(player);
            }

            if (_spawnedWeapons.TryGetValue(player, out NetworkObject weaponObject))
            {
                _networkRunner.Despawn(weaponObject);
                _spawnedWeapons.Remove(player);
            }
        }

        private void SpawnHandler(PlayerRef player)
        {
            if (_networkRunner.IsServer)
            {
                SpawnPlayer(player);
                
                _statisticsPlayers.InitPlayers(player.PlayerId);
                
                _waveController.Init(player.PlayerId, _spawnedCharacters[player].transform, _networkRunner.SessionInfo.PlayerCount);
            }

            if (_networkRunner.SessionInfo.PlayerCount == _maxPlayers)
            {
                _waveController.StartWave();
            }
        }
        
        private void SpawnPlayer(PlayerRef player)
        {
            int weaponNumber = Random.Range(0, _weaponDatas.Count);

            NetworkObject gun = _playerGuns[weaponNumber];
            
            NetworkObject skinPlayer = null;

            foreach (var prefab in _playerPrefab)
            {
                if (prefab.gameObject.name == _playerPrefsSkin)
                {
                    skinPlayer = prefab;
                }
            }
            
            Vector3 spawnPosition = new Vector3((player.RawEncoded % _networkRunner.Config.Simulation.PlayerCount) * 3, 1, 0);
            
            NetworkObject networkGunObject = _networkRunner.Spawn(gun, spawnPosition, Quaternion.identity, player);
            
            NetworkObject networkPlayerObject = _networkRunner.Spawn(skinPlayer, spawnPosition, Quaternion.identity, player, ((runner, o) =>
            {
                o.GetComponent<WeaponController>().Init(_weaponDatas[weaponNumber], networkGunObject, _bulletsView);
                o.GetComponent<PlayerHealthSystem>().Init(_healthView);
                o.GetComponent<MotionHandler>().Init(_joystiksView);
            }));

            _playerGuns.RemoveAt(weaponNumber);
            _weaponDatas.RemoveAt(weaponNumber);
            _spawnedCharacters.Add(player, networkPlayerObject);
            _spawnedWeapons.Add(player, networkGunObject);
            
        }
        
        [Rpc]
        private void RPC_ChangeSkin(string skinName)
        {
            if (string.IsNullOrEmpty(skinName))
            {
                _playerPrefsSkin = Constants.SKIN_BY_DEFAULT;
            }
            else
            {
                _playerPrefsSkin = skinName;
            }
            
        }
    }
}