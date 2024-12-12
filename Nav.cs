using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
public class Nav : MonoBehaviour
{
    [Range(0, 50)][SerializeField] float attackRange = 20, sightRange = 20, timeBetweenAttacks = 3;

    [Range(0, 20)]
    [SerializeField] int power;


    private NavMeshAgent thisCritter;
    private Transform playerPos;


    private bool isAttacking;




    private void Start()
    {
        thisCritter = GetComponent<NavMeshAgent>();
        playerPos = FindAnyObjectByType<Health>().transform;
    }

    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(playerPos.position, this.transform.position);


        if (distanceFromPlayer <= sightRange && distanceFromPlayer > attackRange && !Health.isDead)
        {
            isAttacking = false;
            thisCritter.isStopped = false;
            StopAllCoroutines();

            ChasePlayer();
        }
        if (distanceFromPlayer <= attackRange && !isAttacking && !Health.isDead)
        {
            thisCritter.isStopped = true;
            StartCoroutine(AttackPlayer());
        }

        if (Health.isDead)
        {
            thisCritter.isStopped = true;
        }
    }

    private void ChasePlayer()
    {
        thisCritter.SetDestination(playerPos.position);
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        yield return new WaitForSeconds(timeBetweenAttacks);

        FindAnyObjectByType<Health>().TakeDamage(power);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);

    }
}
