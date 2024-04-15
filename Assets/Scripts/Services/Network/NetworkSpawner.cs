﻿using System.Collections.Generic;
using Fusion;
using Player;
using UI;
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
        
        [SerializeField] private StatisticsPlayersData _statisticsPlayers;
        [SerializeField] private WaveController _waveController;

        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        private Dictionary<PlayerRef, NetworkObject> _spawnedWeapons = new Dictionary<PlayerRef, NetworkObject>();
        
        private string _skinName;
        private int _maxPlayers = 2;
        
        public void PlayerJoined(PlayerRef player)
        {
            if (_networkRunner.IsServer)
            {
                RPC_SetSkinName();
                SpawnPlayer(player);
                
                _statisticsPlayers.InitPlayers(player.PlayerId);
                
                _waveController.Init(player.PlayerId, _spawnedCharacters[player].transform, _networkRunner.SessionInfo.PlayerCount);
            }

            if (_networkRunner.SessionInfo.PlayerCount == _maxPlayers)
            {
                _waveController.StartWave();
            }
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

        private void SpawnPlayer(PlayerRef player)
        {
            int weaponNumber = Random.Range(0, _weaponDatas.Count);

            NetworkObject gun = _playerGuns[weaponNumber];
            
            NetworkObject skinPlayer = new NetworkObject();

            if (_skinName == "") _skinName = ChangeSkin.SKIN_BY_DEFAULT;

            foreach (var prefab in _playerPrefab)
            {
                if (prefab.gameObject.name == _skinName)
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
            }));

            _playerGuns.RemoveAt(weaponNumber);
            _weaponDatas.RemoveAt(weaponNumber);
            _spawnedCharacters.Add(player, networkPlayerObject);
            _spawnedWeapons.Add(player,networkGunObject);
        }

        [Rpc]
        private void RPC_SetSkinName()
        {
            _skinName = PlayerPrefs.GetString(ChangeSkin.PLAYER_PREFS_SKIN);
        }
    }
}