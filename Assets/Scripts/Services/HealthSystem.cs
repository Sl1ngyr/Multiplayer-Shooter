using Fusion;
using UnityEngine;

namespace Services
{
    public abstract class HealthSystem : NetworkBehaviour
    {
        [SerializeField] protected int MaxHealth;
        
        protected int CurrentHealth;
        
        
        public override void Spawned()
        {
            CurrentHealth = MaxHealth;
        }
        
        protected abstract void TakeDamage(int damage);
    }
}