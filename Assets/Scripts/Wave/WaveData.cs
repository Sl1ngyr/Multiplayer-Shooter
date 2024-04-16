using System.Collections.Generic;
using Enemy;
using Items;
using UnityEngine;

namespace Wave
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData")]
    public class WaveData : ScriptableObject
    {
        [field: SerializeField] public float Break { get; private set;}
        [field: SerializeField] public float Duration { get; private set;}
        [field: SerializeField] public List<BaseEnemyController> Enemies { get; private set;}
        [field: SerializeField] public List<BaseItem> Items { get; private set;}
        [field: SerializeField] public float DelayToSpawnEnemy { get; private set;}
        [field: SerializeField] public float DelayToSpawnItems { get; private set;}
    }
}