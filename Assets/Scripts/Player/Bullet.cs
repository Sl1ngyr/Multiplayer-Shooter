using Fusion;
using UnityEngine;

public enum BulletOwner
{
    Player,
    Enemy
}

namespace Player
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private float _speed = 10f;

        [Networked] private TickTimer life { get; set; }
        
        private int _damage;
        private float _turn;

        public BulletOwner _BulletOwner;

        public int Damage => _damage;
        
        public void Init(int damage, float despawnTime, float turn)
        {
            _turn = turn;
            _damage = damage;
            life = TickTimer.CreateFromSeconds(Runner, despawnTime);
        }

        public override void FixedUpdateNetwork()
        {
            if (life.Expired(Runner))
                Runner.Despawn(Object);
            else
                transform.position += (transform.right * _turn) * Runner.DeltaTime * _speed;
        }
    }
}