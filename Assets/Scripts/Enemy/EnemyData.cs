using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set;}
        [field: SerializeField] public int Damage { get; private set;}
        [field: SerializeField] public int HP { get; private set;}
        [field: SerializeField] public float AttackDelay { get; private set;}
        
    }
}