using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : NetworkBehaviour
    {
        private List<NetworkObject> _enemies = new List<NetworkObject>();

        public void SpawnEnemy(Transform target, BaseEnemyController enemy, Vector2 position)
        {
            NetworkObject enemyObject = Runner.Spawn(enemy.gameObject, position, Quaternion.identity, null, ((runner, o) =>
            {
                o.GetComponent<BaseEnemyController>().Init(target);
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