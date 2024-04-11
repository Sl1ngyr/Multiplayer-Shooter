using Fusion;
using Services.Network;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private EnemyRange _enemyRange;
        [SerializeField] private NetworkSpawner _networkSpawner;
        
        public void SpawnEnemy()
        {
            Transform targetPos = new RectTransform();
            foreach (var target in _networkSpawner.Players)
            {
                targetPos = target.Value.transform;
            }
            
            Runner.Spawn(_enemyRange, transform.position, transform.rotation, Object.InputAuthority, ((runner, o) =>
            {
                o.GetComponent<EnemyRange>().Init(targetPos);
            }));
        }
    }
}