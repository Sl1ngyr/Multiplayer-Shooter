using System.Collections.Generic;
using Fusion;
using UI;
using UnityEngine;
using Enemy;
using Player.Weapon;

namespace Services.Network
{
    public class NetworkSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] private List<NetworkObject> _playerPrefab;
        [SerializeField] private List<WeaponData> _weaponDatas;
        [SerializeField] private NetworkRunner _networkRunner;
        [SerializeField] private List<NetworkObject> _playerGuns;
        [SerializeField] private EnemySpawner _enemySpawner;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        private Dictionary<PlayerRef, NetworkObject> _spawnedWeapons = new Dictionary<PlayerRef, NetworkObject>();
        
        private string _skinName;

        public Dictionary<PlayerRef, NetworkObject> Players => _spawnedCharacters;

        public void PlayerJoined(PlayerRef player)
        {
            if (_networkRunner.IsServer)
            {
                RPC_SetSkinName();
                SpawnPlayer(player);
                _enemySpawner.SpawnEnemy();
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
            _playerGuns.RemoveAt(weaponNumber);
            
            Vector2 spawnPosition = Vector2.zero;
            
            NetworkObject skinPlayer = new NetworkObject();
            
            foreach (var prefab in _playerPrefab)
            {
                if (prefab.gameObject.name == _skinName)
                {
                    skinPlayer = prefab;
                }
            }
            
            NetworkObject networkPlayerObject = _networkRunner.Spawn(skinPlayer, spawnPosition, Quaternion.identity, player);
            NetworkObject networkGunObject = _networkRunner.Spawn(gun, spawnPosition, Quaternion.identity, player);

            networkPlayerObject.GetComponent<WeaponController>().InitWeaponData(_weaponDatas[weaponNumber], networkGunObject);
            
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