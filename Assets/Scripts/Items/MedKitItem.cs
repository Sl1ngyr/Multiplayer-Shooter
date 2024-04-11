using Player;
using UnityEngine;

namespace Items
{
    public class MedKitItem : BaseItem
    {
        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.TryGetComponent(out PlayerHealthSystem player))
            {
                player.RestoreHealth();
                
                Runner.Despawn(Object);
            }
        }
    }
}