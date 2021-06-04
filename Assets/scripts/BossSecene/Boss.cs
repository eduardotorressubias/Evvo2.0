using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{

    public float health;

    // rango ataque
    private bool  playerInDistanceRange;
    public float distanceRange;
    public LayerMask whatIsPlayer;

    

    // atacks
    public bool alreadyAttack = false;
    public float TimeBetweenAttacks;
    public GameObject vfx_damage;
    private float timeCounter = 0, tiempo = 0, animacion = 5f, cdTime = 0.7f;
    private bool coldown = false;


    // box de ataque puños
    public GameObject atack, atack2, vfx_atack;
    public Animator animator;


    //Animations
    private bool idle, atack_normal1, atack_normal2, atack_doble, atack_area, dead=false; 

    private void Awake()
    {

        //player = FindObjectOfType<PlayerController>();
        //animator = GetComponentInChildren<Animator>();
        //timeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerInDistanceRange = Physics.CheckSphere(transform.position, distanceRange, whatIsPlayer);
       
        if (!dead)
        {
            if(!playerInDistanceRange )
            {
                Debug.Log("no esta en rango");
                atack_doble = false;
                idle = true;
                
            }
            if(playerInDistanceRange)
            {
                Debug.Log("esta en rango");
                DistanceAtack();
            }
            
        }
        animations();

        if (health <= 0)
        {
            tiempo += Time.deltaTime;
            if (tiempo >= animacion)
            {
                atack_doble = false;
                dead = true;
                transform.rotation = Quaternion.LookRotation(Vector3.zero);
                DestroyEnemy();
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
        if (!dead)
        {
            Debug.Log("aber");
            yield return new WaitForSeconds(4f);
            Debug.Log("funcionaaaa");
            atack.SetActive(true);
            atack2.SetActive(true);
            //yield return new WaitForSeconds(0.5f);
           // vfx_atack.SetActive(true);
        }
        else
        {

        }
    }

    private IEnumerator AttackFalseDistance()
    {
        yield return new WaitForSeconds(8.18f);
        atack.SetActive(false);
        atack2.SetActive(false);
        // vfx_atack.SetActive(false);
        Debug.Log("falsooo");
        atack_doble = false;
        idle = true;
    }

    public void playerColdown()
    {
        if (coldown == true)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= cdTime)
            {
                timeCounter = 0;
                coldown = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (coldown == false)
        {
            if (other.tag == "Attack")
            {
                Debug.Log("Me ha dado");
                vfx_damage.SetActive(false);
                vfx_damage.SetActive(true);
                TakeDamage(1);
                coldown = true;
            }
        }

    }

    private void ResetAttack()
    {
        alreadyAttack = false;
    }

    void animations()
    {
        if (idle)
        {
            animator.SetBool("idle", true);
        }
        else
        {
            animator.SetBool("idle", false);
        }
        if (atack_doble)
        {
            animator.SetBool("attack_doble", true);
        }
        else
        {
            animator.SetBool("attack_doble", false);
        }
        if (dead)
        {
            animator.SetBool("dead", true);
        }
        else
        {
            animator.SetBool("dead", false);
        }
        

    }

    public void TakeDamage(int damage)
    {
        idle = false;
        dead = false;
        health -= damage;
        if (health <= 0)
        {
            dead = true;
            Destroy(gameObject, 5f);
        }
        
    }

    private void DestroyEnemy()
    {

        Debug.Log(dead);
        Destroy(gameObject);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceRange-10);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, meleRange);

    }
}
