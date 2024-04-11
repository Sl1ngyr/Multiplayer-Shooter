using Fusion;
using UnityEngine;

namespace Items
{
    public class BaseItem : NetworkBehaviour
    {
        [SerializeField] private float _despawnTime;
        
        [Networked] private TickTimer TimerToDespawn { get; set; }
        
        public override void Spawned()
        {
            TimerToDespawn = TickTimer.CreateFromSeconds(Runner, _despawnTime);
        }

        public override void FixedUpdateNetwork()
        {
            if (TimerToDespawn.Expired(Runner))
                Runner.Despawn(Object);
        }
    }
}