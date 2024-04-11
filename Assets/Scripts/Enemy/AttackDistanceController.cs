using Fusion;
using Player;
using UnityEngine;

namespace Enemy
{
    public class AttackDistanceController : NetworkBehaviour
    {
        [SerializeField] private BaseEnemyController _enemyController;

        public void OnTriggerEnter2D(Collider2D coll)
        {
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

    }
}