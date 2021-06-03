using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public float health;
    // rango ataque
    public bool playerInMeleRange, playerInDistanceRange, playerInSightRange;
    public float meleRange, distanceRange, sightRange;
    public LayerMask whatIsPlayer;

    // atacks
    public bool alreadyAttack;
    public float TimeBetweenAttacks;
    public GameObject Vfx_damage;
    // box de ataque puños
    public GameObject atack;

    //Animations
    private bool idle, atack_normal1, atack_normal2, atack_doble, atack_area, dead; 

    private void Awake()
    {

        //player = FindObjectOfType<PlayerController>();
        //animator = GetComponentInChildren<Animator>();
        //timeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInDistanceRange = Physics.CheckSphere(transform.position, distanceRange, whatIsPlayer);
        playerInMeleRange = Physics.CheckSphere(transform.position, meleRange, whatIsPlayer);

        if (!dead)
        {
            if(!playerInDistanceRange && !playerInMeleRange)
            {
                idle = true;
            }
            if(playerInDistanceRange && !playerInMeleRange)
            {
                DistanceAtack();
            }
            if(playerInDistanceRange && playerInMeleRange)
            {
                MeleAtack();
            }
        }


    }

    private void DistanceAtack()
    {
        if (!alreadyAttack && dead == false)
        {

            idle = false;
            atack_doble = true;
            //Attack code here
            StartCoroutine(AttackDistanceOn());

            //
            alreadyAttack = true;
            StartCoroutine(AttackFalseDistance());
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);

        }
    }
    private IEnumerator AttackDistanceOn()
    {
        yield return new WaitForSeconds(0f);
        if (!dead)
        {
            atack.SetActive(true);
        }
        else
        {

        }
    }

    private IEnumerator AttackFalseDistance()
    {
        yield return new WaitForSeconds(1.6f);
        atack.SetActive(false);
        atack_doble = false;
        idle = true;
    }

    private void MeleAtack()
    {

    }

    private void ResetAttack()
    {
        alreadyAttack = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleRange);

    }
}
