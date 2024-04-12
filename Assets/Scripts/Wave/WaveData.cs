using System.Collections.Generic;
using Enemy;
using Items;
using UnityEngine;

namespace Wave
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData")]
    public class WaveData : ScriptableObject
    {
        [SerializeField] private float _break;
        [SerializeField] private float _duration;
        [SerializeField] private List<BaseEnemyController> _enemies;
        [SerializeField] private List<BaseItem> _items;
        [SerializeField] private float _delayToSpawnEnemy;
        [SerializeField] private float _delayToSpawnItems;
        
        public float Break => _break;
        public float Duration => _duration;
        public List<BaseEnemyController> Enemies => _enemies;
        public List<BaseItem> Items => _items;
        public float DelayToSpawnEnemy => _delayToSpawnEnemy;
        public float DelayToSpawnItems => _delayToSpawnItems;
    }
}