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
            SetNewTarget(coll.transform);
            
            if (coll.transform.TryGetComponent(out MotionHandler player))
            {
                _enemyController.ReachTarget = true;
            }
        }

        public void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.transform.TryGetComponent(out MotionHandler player))
            {
                _enemyController.ReachTarget = false;
            }
        }

        private void SetNewTarget(Transform transform)
        {
            _enemyController.TargetTransform = transform;
        }
        
    }
}