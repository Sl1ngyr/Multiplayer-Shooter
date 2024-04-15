using Enemy;
using Fusion;
using Player;
using UnityEngine;

namespace Services
{
    public enum BulletOwner
    {
        Player,
        Enemy
    }

    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private float _speed = 10f;

        [Networked] private TickTimer life { get; set; }
        
        private int _damage;
        private float _turn;
        private int _ownerId;
        
        public BulletOwner BulletOwner;

        public int Damage => _damage;
        public int BulletOwnerId => _ownerId;
        
        public void Init(int damage, float despawnTime, float turn)
        {
            _turn = turn;
            _damage = damage;
            life = TickTimer.CreateFromSeconds(Runner, despawnTime / _speed);
            
            _ownerId = Object.InputAuthority.PlayerId;
        }

        public override void FixedUpdateNetwork()
        {
            if (life.Expired(Runner))
                Runner.Despawn(Object);
            else
                transform.position += (transform.right * _turn) * Runner.DeltaTime * _speed;
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.TryGetComponent(out BaseEnemyController enemy))
            {
                if(BulletOwner != BulletOwner.Player) return;
                else if (Object != null)
                {
                    Runner.Despawn(Object);
                }

            }

            if (coll.TryGetComponent(out PlayerHealthSystem player))
            {
                if(BulletOwner != BulletOwner.Enemy) return;
                else if(Object != null)
                {
                    Runner.Despawn(Object);
                }
                
            }
        }
    }
}