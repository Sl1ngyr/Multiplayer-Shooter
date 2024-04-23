using Enemy;
using Fusion;
using Items.AnimationStates;
using Services;
using UnityEngine;

namespace Items
{
    public class BombItem : BaseItem
    {
        [SerializeField] private float _timeToDetonate = 1f;
        private Animator _animator;
        private AnimationBehavior _animationBehavior;
        private bool _hasDetonated = false;
        private bool _isStartDetonate = false;

        public bool IsBombDetonate => _hasDetonated;
        
        public override void Spawned()
        {
            _animator = GetComponent<Animator>();
        }

        public override void FixedUpdateNetwork()
        {
            if (TimerToDespawn.Expired(Runner) && !_isStartDetonate && !_hasDetonated)
            {
                Runner.Despawn(Object);
            }
            else if (TimerToDespawn.Expired(Runner) && _isStartDetonate)
            {
                _hasDetonated = true;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.TryGetComponent(out BaseEnemyController enemy) && !_isStartDetonate)
            {
                StartDetonate();
            }
        }

        private void OnTriggerStay2D(Collider2D coll)
        {
            if (coll.TryGetComponent(out BaseEnemyController enemy))
            {
                if (_hasDetonated)
                {
                    Runner.Despawn(Object);
                }
            }
        }

        private void StartDetonate()
        {
            _isStartDetonate = true;
            TimerToDespawn = TickTimer.CreateFromSeconds(Runner, _timeToDetonate);
            
            _animationBehavior = new AnimationBehaviorBombExplosion(_animator);
            _animationBehavior.Enter();
        }
        
    }
}