using UnityEngine;

public enum ShotType
{
    Single,
    Shotgun
}

namespace Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private float _attackDistance;
        [SerializeField] private float _damage;
        [SerializeField] private float _shootDelay;
        [SerializeField] private ShotType _shotType;

        public Sprite Sprite => _sprite;
        public float AttackDistance => _attackDistance;
        public float Damage => _damage;
        public float ShootDelay => _shootDelay;
        public ShotType ShootTypeWeapon => _shotType;
    }
}