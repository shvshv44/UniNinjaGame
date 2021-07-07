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

    private int currentHealth;
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
        currentHealth = health;
        currentMode = KnightMode.NORMAL;
        currentAnimationState = KnightAnimationState.IDLE;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isAlive)
        {
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
        // TODO:
        rb.velocity = Vector3.zero;
        Debug.Log("ATTACK!!");
    }

    private void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, aggressiveTarget.transform.position) > minimumAttackDistance)
        {
            nav.SetDestination(aggressiveTarget.transform.position);
            SetAnimationState(KnightAnimationState.MOVE);
        } else
        {
            SetAnimationState(KnightAnimationState.ATTACK);
            AttackPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Shuriken" && isAlive)
        {
            TakeDamage();
        }
    }

    private void ChangeMode(KnightMode newMode)
    {
        currentMode = newMode;
    }

    private void TakeDamage()
    {
        currentHealth--;
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
                anim.SetInteger("TriggerNumber", 2);
                anim.SetTrigger("Trigger");
            }
        }
    }

}
