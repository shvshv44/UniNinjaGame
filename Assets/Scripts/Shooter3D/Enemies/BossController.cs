using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{

    private enum BossAnimationState
    {
        IDLE,
        MOVE,
        ATTACK,
        DAMAGE,
        DIE
    }

    private enum BossMode
    {
        NORMAL,
        AGGRESSIVE
    }

    public int health;
    public Transform viewOfSight;
    public float minimumAttackDistance;
    public float attackingCooldown;
    public float damageCooldown;
    public int attackDamage;
    public PlayerStats playerStats;

    private int currentHealth;
    private float currentAttackingCooldown;
    private float currentDamageCooldown;
    private Animator anim;
    private Rigidbody rb;
    private NavMeshAgent nav;
    private bool isAlive;
    private BossMode currentMode;
    private BossAnimationState currentAnimationState;
    private GameObject aggressiveTarget;
    private bool canDamage;
    private bool canBeHit;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        isAlive = true;
        currentAttackingCooldown = 0;
        currentDamageCooldown = 0;
        currentHealth = health;
        currentMode = BossMode.NORMAL;
        currentAnimationState = BossAnimationState.IDLE;
        canDamage = true;
        canBeHit = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlive)
        {
            UpdateAttackingCooldown();
            UpdateDamageCooldown();

            if (currentMode == BossMode.NORMAL)
            {
                SearchForPlayer();
            }
            else if (currentMode == BossMode.AGGRESSIVE)
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
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(viewOfSight.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                aggressiveTarget = hit.collider.gameObject;
                ChangeMode(BossMode.AGGRESSIVE);
            }
            else
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
        if (canDamage)
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
            SetAnimationState(BossAnimationState.MOVE);
        }
        else
        {
            nav.SetDestination(transform.position);
            SetAnimationState(BossAnimationState.ATTACK);
            AttackPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Shuriken" && canBeHit && isAlive)
        {
            int damage = collision.gameObject.GetComponent<Shuriken3DController>().damage;
            TakeDamage(damage);
        }
    }

    private void ChangeMode(BossMode newMode)
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
        SetAnimationState(BossAnimationState.DIE);
        Destroy(gameObject);
    }

    private void SetAnimationState(BossAnimationState state)
    {
        if (state != currentAnimationState)
        {
            currentAnimationState = state;

            if (state == BossAnimationState.IDLE)
            {
                anim.SetBool("Moving", false);
            }
            else if (state == BossAnimationState.MOVE)
            {
                anim.SetBool("Moving", true);
            }
            else if (state == BossAnimationState.ATTACK)
            {
                anim.SetBool("IsAttacking", true);
                anim.SetTrigger("Attack");
                currentAttackingCooldown = attackingCooldown;
            }
            else if (state == BossAnimationState.DAMAGE)
            {
                anim.SetBool("IsDamage", true);
                anim.SetTrigger("Damage");
                currentAttackingCooldown = attackingCooldown;
            }
            else if (state == BossAnimationState.DIE)
            {
                anim.SetBool("IsDie", true);
                anim.SetTrigger("Die");
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

    private void UpdateDamageCooldown()
    {
        if (currentDamageCooldown > 0)
        {
            currentDamageCooldown -= Time.deltaTime;

            if (currentDamageCooldown <= 0)
            {
                anim.SetBool("IsDamage", false);
                canBeHit = true;
                currentDamageCooldown = 0;
            }
        }
    }
}
