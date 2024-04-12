using System.Collections.Generic;
using Enemy;
using Fusion;
using Services.Network;
using UnityEngine;

namespace Wave
{
    public class WaveController : NetworkBehaviour
    {
        [SerializeField] private List<WaveData> _waveDatas;
        [SerializeField] private NetworkSpawner _networkSpawner;
        [SerializeField] private Vector2 _spawnPosition;
        [SerializeField] private TimerWaveController timerWaveController;
        
        [Networked] private TickTimer _spawnEnemy { get; set; }
        [Networked] private TickTimer _spawnItems { get; set; }
        
        [SerializeField] private EnemySpawner _enemySpawner;
        
        private int _currentWave = 0;
        private bool _isRunning = false;

        public bool IsWaveLaunch => _isRunning;

        public override void Spawned()
        {
            timerWaveController.EndWave += ChangeWave;
            timerWaveController.Init(_waveDatas[_currentWave].Break, _waveDatas[_currentWave].Duration);
            
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
            
            if(!timerWaveController.IsStartWave) return;

            if (_spawnEnemy.ExpiredOrNotRunning(Runner))
            {
                SpawnRandomEnemy();
                _spawnEnemy = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].DelayToSpawnEnemy);
            }
        }

        private void SpawnRandomEnemy()
        {
            var randomEnemyNumber = Random.Range(0, _waveDatas[_currentWave].Enemies.Count - 1);
            
            List<Transform> playerTransforms = new List<Transform>();
            
            foreach (var playerTranform in _networkSpawner.Players)
            {
                playerTransforms.Add(playerTranform.Value.transform);
            }

            var randomTarget = Random.Range(0, playerTransforms.Count - 1);

            _enemySpawner.SpawnEnemy(playerTransforms[randomTarget], _waveDatas[_currentWave].Enemies[randomEnemyNumber], GetRandomPositionForSpawn());
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
            timerWaveController.Init(_waveDatas[_currentWave].Break, _waveDatas[_currentWave].Duration);
            
            _enemySpawner.DestroyAllEnemies();

            _spawnEnemy = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].DelayToSpawnEnemy);
            _spawnItems = TickTimer.CreateFromSeconds(Runner, _waveDatas[_currentWave].DelayToSpawnItems);
        }
        
        private void OnDisable()
        { 
            timerWaveController.EndWave -= ChangeWave;
        }
        
    }
}