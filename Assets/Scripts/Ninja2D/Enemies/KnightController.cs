using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{

    public static class KnightState
    {
        public const string Walk = "IsWalk";
        public const string Idle = "IsIdle";
        public const string Die = "IsDie";
    }

    private enum KnightMode
    {
        NORMAL,
        AGGRESSIVE,
        DIE
    }

    public int lives = 1;
    public int speed = 1;
    public AudioClip hurtSound;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerHarmable playerHarmable;
    private AudioSource audioSrc;

    private GameObject aggressiveTarget;
    private KnightMode currentMode;
    private string currentState;
    private float volume = 0.8f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerHarmable = GetComponent<PlayerHarmable>();
        audioSrc = GetComponent<AudioSource>();
        currentMode = KnightMode.NORMAL;
    }

    private void FixedUpdate()
    {
        if (currentMode == KnightMode.DIE)
        {
            Destroy(playerHarmable);
            SetState(KnightState.Die);
            StartCoroutine(WaitAndDie());
        }
        else if (currentMode == KnightMode.NORMAL)
        {
            rb.velocity = Vector2.zero;
            SetState(KnightState.Idle);
        } else
        {
            SetState(KnightState.Walk);
            AggressiveWalk();
        }
    }

    public void SetNormalMode()
    {
        if(currentMode != KnightMode.DIE)
            currentMode = KnightMode.NORMAL;
    }

    public void SetAggressiveMode(GameObject go)
    {
        if (currentMode != KnightMode.DIE)
        {
            aggressiveTarget = go;
            currentMode = KnightMode.AGGRESSIVE;
        }
    }

    public void SetDieMode()
    {
        currentMode = KnightMode.DIE;
    }

    private void SetState(string state)
    {
        if (state != currentState)
        {
            currentState = state;
            animator.SetBool(KnightState.Idle, false);
            animator.SetBool(KnightState.Walk, false);
            animator.SetBool(KnightState.Die, false);
            animator.SetBool(state, true);
        }
    }

    private IEnumerator WaitAndDie()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void AggressiveWalk()
    {
        if (rb.position.x > aggressiveTarget.transform.position.x)
        {
            // Aggessive walk left
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            rb.AddForce(new Vector2(-speed * Time.fixedDeltaTime, 0));
        } else
        {
            // Aggessive walk right
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            rb.AddForce(new Vector2(speed * Time.fixedDeltaTime, 0));
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shuriken")
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        lives--;
        audioSrc.PlayOneShot(hurtSound, volume);
        if(lives == 0)
        {
            SetDieMode();
        }
    }
}
