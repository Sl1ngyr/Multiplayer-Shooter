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
            if(!CheckColliderIsOurTarget(coll.transform)) return;
            
            if (coll.transform.TryGetComponent(out MotionHandler player))
            {
                _enemyController.ReachTarget = true;
            }
        }

        public void OnTriggerExit2D(Collider2D coll)
        {
            if(!CheckColliderIsOurTarget(coll.transform)) return;
            
            if (coll.transform.TryGetComponent(out MotionHandler player))
            {
                _enemyController.ReachTarget = false;
            }
        }

        private bool CheckColliderIsOurTarget(Transform transform)
        {
            if (_enemyController.GetTargetTransform != transform) return false;
            else return true;
        }
        
    }
}