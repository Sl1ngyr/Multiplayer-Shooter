using Fusion;
using UnityEngine;

namespace Player
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private float _speed = 10f;

        [Networked] private TickTimer life { get; set; }

        private float _damage;
        private float _turn;
        
        public void Init(float damage, float despawnTime, float turn)
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