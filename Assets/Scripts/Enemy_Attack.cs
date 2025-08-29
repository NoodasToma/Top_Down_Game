using UnityEngine;
using Combat;
using System.Collections;
public class Enemy_Attack : MonoBehaviour
{
    [Header("Attack Settings")]

    public Combo enemyAttacks;
 
    [Header("Debug")]
    public bool showGizmos = true;

    private float lastClickTime;

    private Animator enemyAttackAnimator;

    private int attackIndex;


    private bool isAttacking;







    void Start()
    {
        enemyAttackAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time >= lastClickTime + enemyAttacks.GetComboResetTime) attackIndex = 0;
    }

    public void Attack()
    {
        
        if (attackIndex > enemyAttacks.GetAttacks.Length - 1)
            {
                attackIndex = 0;
            StartCoroutine(comboCd());
            }

        if (Time.time >= lastClickTime + enemyAttacks.GetCooldownBetweenAttacks && !isAttacking)
        {

            enemyAttackAnimator.SetTrigger(enemyAttacks.GetAttacks[attackIndex].AnimationValue);
            lastClickTime = Time.time;
            attackIndex++;
            isAttacking = true;

        }


    }

    IEnumerator comboCd()
    {
        isAttacking = true;
        yield return new WaitForSeconds(enemyAttacks.GetCooldownBetweenCombos);
        isAttacking = false;
    }



    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyAttacks.GetAttacks[attackIndex].RangeValue);


    }

    public void attackExecutor()
    {
        enemyAttacks.startCombo(gameObject, attackIndex);
        isAttacking = false;
    }

}