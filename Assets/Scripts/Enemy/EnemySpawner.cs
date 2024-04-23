using System.Collections.Generic;
using Fusion;
using Services;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private StatisticsPlayersData _statisticsPlayers;
        
        private List<NetworkObject> _enemies = new List<NetworkObject>();

        public void RecordDamageFromPlayer(int id, int damage)
        {
            _statisticsPlayers.AddPlayerDamageToData(id, damage);
        }
        
        public void RecordKillFromPlayer(int id)
        {
            _statisticsPlayers.AddPlayerKillsToData(id);
        }
        
        public void SpawnEnemy(List<Transform> targets, BaseEnemyController enemy, Vector2 position)
        {
            NetworkObject enemyObject = Runner.Spawn(enemy.gameObject, position, Quaternion.identity, null, ((runner, o) =>
            {
                o.GetComponent<BaseEnemyController>().Init(targets);
                o.GetComponent<EnemyHealthSystem>().Init(this);
            }));
            
            _enemies.Add(enemyObject);
        }
        
        public void DestroyAllEnemies()
        {
            foreach (var enemy in _enemies)
            {
                if(enemy == null) continue;
                
                Runner.Despawn(enemy);
            }
            
            _enemies.Clear();
        }
    }
}