using System.Collections.Generic;
using Fusion;
using UI;
using UnityEngine;
using Weapon;

namespace Services
{
    public class NetworkSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] private List<NetworkObject> _playerPrefab;
        [SerializeField] private List<WeaponData> _weaponDatas;
        [SerializeField] private NetworkRunner _networkRunner;
        [SerializeField] private List<NetworkObject> _playerGuns;
        
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        private string _skinName;
        
        public void PlayerJoined(PlayerRef player)
        {
            if (_networkRunner.IsServer)
            {
                RPC_SetSkinName();
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
        }

        [Rpc]
        private void RPC_SetSkinName()
        {
            _skinName = PlayerPrefs.GetString(ChangeSkin.PLAYER_PREFS_SKIN);
        }
        
    }
}