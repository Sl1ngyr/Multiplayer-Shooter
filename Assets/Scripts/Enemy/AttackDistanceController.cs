using Fusion;
using Player;
using UnityEngine;

namespace Enemy
{
    public class AttackDistanceController : NetworkBehaviour
    {
        [SerializeField] private BaseEnemyController _enemyController;

        public void OnTriggerStay2D(Collider2D coll)
        {
            SetNewTargetForAttack(coll.transform);
            
            if (coll.transform.TryGetComponent(out MotionHandler player))
            { 
                _enemyController.ReachTarget = true;
            }
        }

        public void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.transform.TryGetComponent(out MotionHandler player))
            {
                if (!coll.enabled)
                {
                    _enemyController.SetNewTarget(player.transform);
                }

                _enemyController.ReachTarget = false;
            }
        }

        private void SetNewTargetForAttack(Transform target)
        {
            if (_enemyController.TargetTransform != target)
            {
                _enemyController.TargetTransform = target;
            }
        }
        
    }
}