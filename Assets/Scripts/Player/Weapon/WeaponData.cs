using UnityEngine;

namespace Player.Weapon
{
    public enum ShotType
    {
        Single,
        Shotgun
    }

    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public float AttackDistance { get; private set;}
        [field: SerializeField] public int Damage { get; private set;}
        [field: SerializeField] public float ShootDelay { get; private set;}
        [field: SerializeField] public ShotType ShootTypeWeapon { get; private set;}
        [field: SerializeField] public int NumberOfBullets { get; private set;}
    }
}