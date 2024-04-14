using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Items
{
    public class ItemSpawner : NetworkBehaviour
    {
        private List<NetworkObject> _items = new List<NetworkObject>();
        
        public void SpawnItems(BaseItem item, Vector2 position)
        {
            NetworkObject itemObject = Runner.Spawn(item.gameObject, position, Quaternion.identity, null);
            
            _items.Add(itemObject);
        }
        
        public void DestroyAllItems()
        {
            foreach (var item in _items)
            {
                if(item == null) continue;
                
                Runner.Despawn(item);
            }
            
            _items.Clear();
        }
    }
}