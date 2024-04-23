using Player.Weapon;
using UnityEngine;

namespace Items
{
    public class AmmunitionBoxItem : BaseItem
    {
        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.TryGetComponent(out WeaponController player))
            {
                player.RestoreAllBullets();
                Runner.Despawn(Object);
            }
            
        }

    }
    
}