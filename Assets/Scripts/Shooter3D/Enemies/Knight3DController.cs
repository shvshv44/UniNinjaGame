using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knight3DController : MonoBehaviour
{

    private enum KnightAnimationState
    {
        IDLE,
        MOVE,
        ATTACK
    }

    private enum KnightMode
    {
        NORMAL,
        AGGRESSIVE
    }

    public int health;
    public Transform viewOfSight;
    public float minimumAttackDistance;
    public float attackingCooldown;
    public int attackDamage;
    public PlayerStats playerStats;
    public bool canDamage;

    private int currentHealth;
    private float currentAttackingCooldown;
    private Animator anim;
    private Rigidbody rb;
    private NavMeshAgent nav;
    private bool isAlive;
    private KnightMode currentMode;
    private KnightAnimationState currentAnimationState;
    private GameObject aggressiveTarget;
    
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        isAlive = true;
        currentAttackingCooldown = 0;
        currentHealth = health;
        currentMode = KnightMode.NORMAL;
        currentAnimationState = KnightAnimationState.IDLE;
        canDamage = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isAlive)
        {
            UpdateAttackingCooldown();

            if(currentMode == KnightMode.NORMAL)
            {
                SearchForPlayer();
            } else if(currentMode == KnightMode.AGGRESSIVE)
            {
                ChasePlayer();
            }
        }
    }

    private void SearchForPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(viewOfSight.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(viewOfSight.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                aggressiveTarget = hit.collider.gameObject;
                ChangeMode(KnightMode.AGGRESSIVE);
            } else
            {
                Debug.DrawRay(viewOfSight.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green);
            }
        }
        else
        {
            Debug.DrawRay(viewOfSight.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green);
        }
    }

    private void AttackPlayer()
    {
        rb.velocity = Vector3.zero;
        if(canDamage)
        {
            playerStats.TakeDamage(attackDamage);
            canDamage = false;
        }
    }

    private void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, aggressiveTarget.transform.position) > minimumAttackDistance)
        {
            nav.SetDestination(aggressiveTarget.transform.position);
            SetAnimationState(KnightAnimationState.MOVE);
        } else
        {
            nav.SetDestination(transform.position);
            SetAnimationState(KnightAnimationState.ATTACK);
            AttackPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Shuriken" && isAlive)
        {
            int damage = collision.gameObject.GetComponent<Shuriken3DController>().damage;
            TakeDamage(damage);
        }
    }

    private void ChangeMode(KnightMode newMode)
    {
        currentMode = newMode;
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }

    private void SetAnimationState(KnightAnimationState state)
    {
        if(state != currentAnimationState)
        {
            currentAnimationState = state;

            if(state == KnightAnimationState.IDLE)
            {
                anim.SetBool("Moving", false);
            } else if(state == KnightAnimationState.MOVE)
            {
                anim.SetBool("Moving", true);
            } else if (state == KnightAnimationState.ATTACK)
            {
                anim.SetBool("IsAttacking", true);
                anim.SetTrigger("Attack");
                currentAttackingCooldown = attackingCooldown;
            }
        }
    }

    private void UpdateAttackingCooldown()
    {
        if (currentAttackingCooldown > 0)
        {
            currentAttackingCooldown -= Time.deltaTime;

            if (currentAttackingCooldown <= 0)
            {
                anim.SetBool("IsAttacking", false);
                canDamage = true;
                currentAttackingCooldown = 0;
            }
        }
    }

}
