using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        [SerializeField] private int _hp;
        [SerializeField] private float _attackDelay;
        
        public float Speed => _speed;
        public float Damage => _damage;
        public int HP => _hp;
        public float AttackDelay => _attackDelay;
    }
}