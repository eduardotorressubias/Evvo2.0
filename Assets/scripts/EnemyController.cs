using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent agent;

    public PlayerController player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;


    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public bool dead = false;


    //Attacking
    public float timeBetweenAttacksMele;
    public float timeBetweenAttacksDistance;
    bool alreadyAttacked;
    public GameObject projectile;
    private Vector3 PosProjectile;
    public float yProject;
    public GameObject atack;

    public GameObject cannon_pos;

    //Animations
    private bool distance;
    private bool mele;
    private bool dmg1;
    private bool dmg2;
    private bool die =false;
    private bool walk;
    private bool idle;
    private Animator animator;
    private float random;
    private float timeCounterCd = 0;
    private float cdTime = 0.5f;
    private float cdTimeDead = 5f;
    private bool coldownDead = false;
    private bool coldown = false;


    //States
    public float sightRange , attackRange, meleAttack ;
    public float TamañoCubo;
    private Vector3 box;
    public Transform evvo;

    public bool playerInSightRange, playerInAttackRange, Soundon, playerInMeleAtack;
    Vector3 playerLook;
    private float timeCounter=0;
    public GameObject bossSound;
    
    public GameObject soul;

    private TriggerSFX sfx;
    private Puerta_final anim;

    private void Awake()
    {
       
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        sfx = FindObjectOfType<TriggerSFX>();
        anim = FindObjectOfType<Puerta_final>();
        animator = GetComponentInChildren<Animator>();
        timeCounter = 0;
    }

    private void Update()
    {
        //Debug.Log("enemy " + transform.position.y + ", player " + player.transform.position.y);
        //check vision y rango de ataque
        box = new Vector3(TamañoCubo, 3f, TamañoCubo);
        Vector3 positionEvvo = evvo.position - transform.position;
        playerInSightRange = Physics.CheckBox(transform.position, box, Quaternion.LookRotation(positionEvvo), whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInMeleAtack = Physics.CheckSphere(transform.position, meleAttack, whatIsPlayer);
        if (!die)
        {
            if (!playerInSightRange && !playerInAttackRange && !playerInMeleAtack)
            {
                idle = false;
                walk = true;
                Patroling();
                Soundon = false;
                timeCounter = 0;
            }
            if (playerInSightRange && !playerInAttackRange && !playerInMeleAtack)
            {
                walk = true;
                ChasePlayer();
                Soundon = true;
                timeCounter++;
            }
            if (playerInSightRange && playerInAttackRange && !playerInMeleAtack)
            {
                walk = false;


                AttackPlayer();
            }
            if (playerInSightRange && playerInAttackRange && playerInMeleAtack)
            {
                walk = false;
                distance = false;
                MeleAttack();
            }
        }
        animations();
        playerColdown();
        playerColdown2();


        if (Soundon && timeCounter == 1 )
        {
            bossSound.SetActive(true);
        }


    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            if (agent.velocity.normalized != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
            }

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //WalkPoint reached
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
            }
        }

    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);

    }
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        playerLook = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        if (!die)
        {
            transform.LookAt(playerLook);
        }
        

        if (!alreadyAttacked && dmg1 == false && dmg2 == false && die == false)
        {
            
            idle = false;
            distance = true;
            //Attack code here
            StartCoroutine(AttackDistanceOn());

            //
            alreadyAttacked = true;
            StartCoroutine(AttackFalse());
            Invoke(nameof(ResetAttack), timeBetweenAttacksDistance);

        }
            
    }
    private void MeleAttack()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        playerLook = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        if (!die)
        {
            transform.LookAt(playerLook);
        }
        

        if (!alreadyAttacked && dmg1 == false && dmg2 == false && die == false)
        {
            
            idle = false;
            mele = true;
            //Attack code here
            StartCoroutine(AttackMeleOn());


            //
            alreadyAttacked = true;

            StartCoroutine(AttackFalse2());

            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacksMele);

        }

    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        idle = false;
        walk = false;
        die = false;

        health -= damage;

        if (health <= 0)
        {
            die = true;
            mele = false;
            distance = false;
            Invoke(nameof(DestroyEnemy), 5f);
        }
        else
        {
            random = Random.Range(1, 3);
            if (random == 1)
            {
                dmg1 = true;
            }
            else if (random == 2)
            {
                dmg2 = true;
            }
        }
        
    }

    private void DestroyEnemy()
    {
       
       // soul.SetActive(true);
        dead = true;
        anim.puertaFinal();
        sfx.SemibossDead();
        Destroy(gameObject);

    }
    public void playerColdown()
    {
        if (coldown == true)
        {
            timeCounterCd += Time.deltaTime;
            if (timeCounterCd >= cdTime)
            {
                timeCounterCd = 0;
                coldown = false;
            }
        }
    }
    public void playerColdown2()
    {
        if (coldownDead == true)
        {
            timeCounterCd += Time.deltaTime;
            if (timeCounterCd >= cdTimeDead)
            {
                DestroyEnemy();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
       if(coldown == false)
        {
            if (other.tag == "Attack")
            {
                Debug.Log("Me ha dado");
                TakeDamage(1);
                coldown = true;
            }
        }
        
    }

    private void animations()
    {

        if (dmg1)
        {
            Invoke(nameof(takeDamage1), 0f);
        }
        else
        {
            animator.SetBool("Dmg1", false);
        }

        if (dmg2)
        {
            Invoke(nameof(takeDamage2), 0f);
        }
        else
        {
            animator.SetBool("Dmg2", false);
        }

        if (mele)
        {
            animator.SetBool("Mele", true);

        }
        else
        {
            animator.SetBool("Mele", false);
        }

        if (distance)
        {
            animator.SetBool("Distance", true);

        }
        else
        {
            animator.SetBool("Distance", false);
        }

        if (walk)
        {
            animator.SetBool("Walk", true);

        }
        else
        {
            animator.SetBool("Walk", false);
        }
        if (die)
        {
            animator.SetBool("Die", true);
        }
        else
        {
            animator.SetBool("Die", false);
        }
        if (idle)
        {
            animator.SetBool("Idle", true);
        }
        else
        {
            animator.SetBool("Idle", false);
        }


    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, box);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleAttack);

    }

    private IEnumerator AttackFalse()
    {
        yield return new WaitForSeconds(1.6f);
        atack.SetActive(false);
        distance = false;
        idle = true;
    }
    private IEnumerator AttackFalse2()
    {
        yield return new WaitForSeconds(3.5f);
        atack.SetActive(false);
        mele = false;
        idle = true;
    }
    private IEnumerator AttackMeleOn()
    {
        yield return new WaitForSeconds(1.45f);
        if (!die)
        {
            atack.SetActive(true);
        }
        else
        {
            atack.SetActive(false);
        }
    }
    private IEnumerator AttackDistanceOn()
    {
        yield return new WaitForSeconds(0.8f);
        if (!die)
        {
            //PosProjectile = new Vector3(transform.position.x-0.2f, transform.position.y + yProject, transform.position.z+0.68f);
            cannon_pos.transform.LookAt(player.transform.position);
            PosProjectile = cannon_pos.transform.position;

            Rigidbody rb = Instantiate(projectile, PosProjectile + (transform.forward), Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(cannon_pos.transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(cannon_pos.transform.up * 10f, ForceMode.Impulse);
        }
        else
        {

        }  
    }
    private void takeDamage1()
    {
        animator.SetBool("Dmg1", true);
        StartCoroutine(DmgFalse());

    }
    private void takeDamage2()
    {
        animator.SetBool("Dmg2", true);
        StartCoroutine(DmgFalse());

    }

    private IEnumerator DmgFalse()
    {
        yield return new WaitForSeconds(0.2f);
        dmg1 = false;
        dmg2 = false;
    }
}
