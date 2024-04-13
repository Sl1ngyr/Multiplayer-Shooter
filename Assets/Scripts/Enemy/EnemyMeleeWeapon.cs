using Enemy.AnimationStates;
using Fusion;
using UnityEngine;

namespace Enemy
{
    public class EnemyMeleeWeapon : NetworkBehaviour
    {
        [SerializeField] private EnemyMelee _enemy;
        
        private Animator _weaponAnimator;
        private BoxCollider2D _boxCollider;
        
        public override void Spawned()
        {
            _weaponAnimator = GetComponent<Animator>();
            _boxCollider = GetComponent<BoxCollider2D>();
            
            _boxCollider.enabled = false;

            _enemy.OnAttacked += Attack;
            _enemy.OnEnemyMeleeDead += DeactivateWeapon;

        }

        public void EndOfAttack()
        {
            _boxCollider.enabled = false;
        }

        private void DeactivateWeapon()
        {
            gameObject.SetActive(false);
        }
        
        private void Attack()
        {
            _boxCollider.enabled = true;
            _weaponAnimator.SetTrigger(DescriptionEnemyAnimation.ENEMY_MELEE_WEAPON_ATTACK);
        }

        private void OnDisable()
        {
            _enemy.OnAttacked -= Attack;
            _enemy.OnEnemyMeleeDead -= DeactivateWeapon;
        }
    }
}