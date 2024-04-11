using Enemy;
using Fusion;
using UnityEngine;

namespace Player
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

        public BulletOwner BulletOwner;

        public int Damage => _damage;
        
        public void Init(int damage, float despawnTime, float turn)
        {
            _turn = turn;
            _damage = damage;
            life = TickTimer.CreateFromSeconds(Runner, despawnTime / _speed);
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
                
                Runner.Despawn(Object);
            }

            if (coll.TryGetComponent(out PlayerHealthSystem player))
            {
                if(BulletOwner != BulletOwner.Enemy) return;
                Runner.Despawn(Object);
            }
        }
    }
}