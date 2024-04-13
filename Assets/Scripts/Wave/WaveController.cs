﻿using System.Collections.Generic;
using Enemy;
using Fusion;
using Player;
using Services.Network;
using UnityEngine;

namespace Wave
{
    public class WaveController : NetworkBehaviour
    {
        [SerializeField] private List<WaveData> _waveDatas;
        [SerializeField] private NetworkSpawner _networkSpawner;
        [SerializeField] private Vector2 _spawnPosition;
        [SerializeField] private TimerWaveController _timerWaveController;
        
        [Networked] private TickTimer _spawnEnemy { get; set; }
        [Networked] private TickTimer _spawnItems { get; set; }
        
        [SerializeField] private EnemySpawner _enemySpawner;
        
        private int _currentWave = 0;
        private int _numberOfLivePlayers;
        private int _maxWave;
        private bool _isRunning = false;
        
        private List<Transform> _playerTransforms = new List<Transform>();
        
        public bool IsWaveLaunch => _isRunning;

        public void Init(Transform transformPlayer, int numberOfPlayers)
        {
            _numberOfLivePlayers = numberOfPlayers;
            
            _playerTransforms.Add(transformPlayer);

            transformPlayer.GetComponent<PlayerHealthSystem>().OnPlayerLoseLifeEvent += RemovePlayerTransform;
        }
        
        public override void Spawned()
        {
            _timerWaveController.EndWave += ChangeWave;
            _timerWaveController.Init(_waveDatas[_currentWave].Break, _waveDatas[_currentWave].Duration);

            _maxWave = _waveDatas.Count;
            
            _spawnEnemy = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].DelayToSpawnEnemy);
            _spawnItems = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].DelayToSpawnItems);
        }

        public override void FixedUpdateNetwork()
        {
            if (_isRunning)
            {
                if (_spawnItems.ExpiredOrNotRunning(Runner))
                {
                    SpawnRandomItems();
                    _spawnItems = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].DelayToSpawnItems);
                }
            }
            
            if(!_timerWaveController.IsStartWave || !_isRunning) return;

            if (_spawnEnemy.ExpiredOrNotRunning(Runner))
            {
                SpawnRandomEnemy();
                _spawnEnemy = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].DelayToSpawnEnemy);
            }
        }

        public void StartWave()
        {
            _isRunning = true;
            
        }
        
        private void RemovePlayerTransform(Transform transform)
        {
            _numberOfLivePlayers--;

            if (_numberOfLivePlayers == 0)
            {
                transform.GetComponent<PlayerHealthSystem>().OnPlayerLoseLifeEvent -= RemovePlayerTransform;
                
                DeactivateWave();
                return;
            }
            
            foreach (var playerTransform in _playerTransforms)
            {
                if (playerTransform == transform)
                {
                    playerTransform.GetComponent<PlayerHealthSystem>().OnPlayerLoseLifeEvent -= RemovePlayerTransform;
                    
                    _playerTransforms.Remove(playerTransform);
                    
                    return;
                }
            }
        }
        
        private void SpawnRandomEnemy()
        {
            var randomEnemyNumber = Random.Range(0, _waveDatas[_currentWave].Enemies.Count - 1);

            _enemySpawner.SpawnEnemy(_playerTransforms, _waveDatas[_currentWave].Enemies[randomEnemyNumber], GetRandomPositionForSpawn());
        }

        private void SpawnRandomItems()
        {
            var randomItemNumber = Random.Range(0, _waveDatas[_currentWave].Items.Count - 1);
            
            Runner.Spawn(_waveDatas[_currentWave].Items[randomItemNumber], GetRandomPositionForSpawn(), Quaternion.identity, null);
        }
        
        private Vector2 GetRandomPositionForSpawn()
        {
            float positionX = Random.Range(0, _spawnPosition.x);
            float positionY = Random.Range(0, _spawnPosition.y);
            
            return new Vector2(positionX, positionY);
        }

        private void ChangeWave()
        {
            _currentWave++;

            if (_currentWave == _maxWave)
            {
                DeactivateWave();
                return;
            }
                
            _timerWaveController.Init(_waveDatas[_currentWave].Break, _waveDatas[_currentWave].Duration);
            
            _enemySpawner.DestroyAllEnemies();

            _spawnEnemy = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].DelayToSpawnEnemy);
            _spawnItems = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].DelayToSpawnItems);
        }
        
        
        private void DeactivateWave()
        {
            _isRunning = false;

            _enemySpawner.DestroyAllEnemies();
        }
        
        private void OnDisable()
        { 
            _timerWaveController.EndWave -= ChangeWave;
        }
        
    }
}