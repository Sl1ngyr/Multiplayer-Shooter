using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Weapon;

namespace Services
{
    public class NetworkSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] private NetworkPrefabRef _playerPrefab;
        [SerializeField] private List<WeaponData> _weaponDatas;
        [SerializeField] private NetworkRunner _networkRunner;
        [SerializeField] private List<NetworkObject> _playerGuns;
        
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        
        public void PlayerJoined(PlayerRef player)
        {
            if (_networkRunner.IsServer)
            {
                SpawnPlayer(player);
            }
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                _networkRunner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }
        
        private void SpawnPlayer(PlayerRef player)
        {
            int weaponNumber = Random.Range(0, _weaponDatas.Count);
            NetworkObject gun = _playerGuns[weaponNumber];
            _playerGuns.RemoveAt(weaponNumber);
            
            Vector2 spawnPosition = Vector2.zero;
            
            NetworkObject networkPlayerObject = _networkRunner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            NetworkObject networkGunObject = _networkRunner.Spawn(gun, spawnPosition, Quaternion.identity, player);

            networkPlayerObject.GetComponent<WeaponController>().InitWeaponData(_weaponDatas[weaponNumber], networkGunObject);
            
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
        
    }
}