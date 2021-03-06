using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class NinjaController : MonoBehaviour
{

    public static class NinjaState
    {
        public const string Jump = "IsJump";
        public const string Walk = "IsWalk";
        public const string Idle = "IsIdle";
        public const string OnWall = "IsOnWall";
        public const string StopWalk = "IsStopWalk";
        public const string Throw = "IsThrow";
    }

    public Animator animator;

    public ShurikenController shuriken;

    public AudioClip throwShurikenSound;
    public AudioClip jumpSound;
    private AudioSource audioSrc;
    private float volume = 0.8f;

    public float shurikenSpeed;
    public float shurikenCooldown;
    private float currentShurikenCooldown = 0;

    private Rigidbody2D RBody { get; set; }

    [SerializeField]
    private PhysicsParams physicsParams;

    public Vector2 Velocity { get { return (RBody.velocity); } }

    public Vector2 VelocityRelativeGround { get { return (Velocity / PhysicsParams.onGroundMaxVelHorizontal); } }

    private float timeRealLastGroundCollision = 0;
    private float timeRealLastWallLeftCollision = 0;
    private float timeRealLastWallRightCollision = 0;

    public bool IsOnGround
    {
        get
        {
            return GetIsColliding(timeRealLastGroundCollision);
        }
    }

    public bool IsOnWallLeft
    {
        get
        {
            return GetIsColliding(timeRealLastWallLeftCollision);
        }
    }

    public bool IsOnWallRight
    {
        get
        {
            return GetIsColliding(timeRealLastWallRightCollision);
        }
    }

    public bool IsInAir { get { return isPlayerInAir; } }

    public string LastState { get { return lastState; } }

    private bool GetIsColliding(float timeLastCollision)
    {
        return (Time.realtimeSinceStartup < timeLastCollision + 0.05f);
    }

    private Vector2 currentVelocity = Vector2.zero;
    private Vector2 currentForce = Vector2.zero;
    private Vector2 hitForce = Vector2.zero;

    private float EntityMass { get { return (PhysicsParams.playerMass); } }

    private bool isPlayerInAir = false;
    private bool keyJumpRetrigger = false;
    private bool keyJumpPressed = false;
    private bool isPlayerOnWall = false;

    private string lastState = NinjaState.Idle;

    public PhysicsParams PhysicsParams
    {
        get { return physicsParams; }
        set { physicsParams = value; }
    }

    public Vector2 CurrentForce { get { return currentForce; } }

    public Vector2 HitForce { get { return hitForce; } }

    public void DefineHitForce(Vector2 force)
    {
        hitForce = force;
    }

    public bool IsOnWall { get { return isPlayerOnWall; } }

    private List<Renderer> allRenderers;

    public List<Renderer> AllRenderers { get { return allRenderers; } }

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public Vector2 Position2D
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public void Awake()
    {
        RBody = GetComponent<Rigidbody2D>();
        allRenderers = new List<Renderer>(GetComponentsInChildren<Renderer>(true));
        audioSrc = GetComponent<AudioSource>();
    }

    public void Update()
    {

        //let's reset forces to 0 and then add regular gravitation
        SimResetForce();
        SimAddForce(new Vector2(0, PhysicsParams.gameGravity) * EntityMass);

        //process key input (like jumping key pressed, etc...)
        ProcessInput();

        //simulate position and velocity based on all acting forces
        ComputeVelocity(Time.deltaTime);

        //collision detection with static world
        isPlayerOnWall = IsOnWallLeft || IsOnWallRight;
        isPlayerInAir = IsOnGround == false;
    }

    private void SimResetForce()
    {
        currentForce = Vector2.zero;
    }

    private void SimAddForce(Vector2 force)
    {
        currentForce += force;
    }

    private void ComputeVelocity(float dt)
    {

        currentVelocity += (currentForce / EntityMass) * dt;

        //let's cap the speed in case its higher than the max
        if (isPlayerInAir)
        {
            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -PhysicsParams.inAirMaxVelHorizontal, PhysicsParams.inAirMaxVelHorizontal);
        }
        else
        {
            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -PhysicsParams.onGroundMaxVelHorizontal, PhysicsParams.onGroundMaxVelHorizontal);
        }

        //Hit force handling
        currentVelocity += hitForce;
        hitForce = Vector2.zero;

        RBody.velocity = currentVelocity;
    }

    private void ProcessInput()
    {

        bool isKeyDownJump = Input.GetButton("Jump");
        bool isKeyDownThrow = Input.GetButton("Fire1");
        float inputAxisX = Input.GetAxisRaw("Horizontal");
        bool isKeyDownLeft = inputAxisX < -0.5f;
        bool isKeyDownRight = inputAxisX > 0.5f;

        //-----------------
        //JUMPING LOGIC:
        //player is on ground
        if (isPlayerInAir == false)
        {
            //in case the player is on ground and does not press the jump key, he
            //should be allowed to jump
            if (isKeyDownJump == false)
            {
                keyJumpRetrigger = true;
            }

            //did player press down the jump button?
            if (isKeyDownJump == true && keyJumpRetrigger == true)
            {
                keyJumpPressed = true;
                keyJumpRetrigger = false;

                //when pressing jump on ground we set the upwards velocity directly
                currentVelocity = new Vector2(currentVelocity.x, PhysicsParams.jumpUpVel);
                audioSrc.PlayOneShot(jumpSound, volume);
            }
        }
        else if (isPlayerOnWall == true)
        {

            //let's allow jumping again in case of being on the wall
            if (isKeyDownJump == false)
            {
                keyJumpRetrigger = true;
            }
            if (currentVelocity.y < 0)
            {//apply friction when moving downwards
                SimAddForce(new Vector2(0, PhysicsParams.wallFriction) * EntityMass);
            }
            if (currentVelocity.y < PhysicsParams.wallFrictionStrongVelThreshold)
            {//apply even more friction when moving downwards fast
                SimAddForce(new Vector2(0, PhysicsParams.wallFrictionStrong) * EntityMass);
            }
            if (isKeyDownJump == true && keyJumpRetrigger == true)
            {
                keyJumpPressed = true;
                keyJumpRetrigger = false;
                audioSrc.PlayOneShot(jumpSound, volume);

                //in case we are moving down -> let's set the velocity directly
                //in case we are moving up -> sum up velocity
                if (IsOnWallLeft == true)
                {
                    if (currentVelocity.y <= 0)
                    {
                        currentVelocity = new Vector2(PhysicsParams.jumpWallVelHorizontal, PhysicsParams.jumpWallVelVertical);
                    }
                    else
                    {
                        currentVelocity = new Vector2(PhysicsParams.jumpWallVelHorizontal, currentVelocity.y + PhysicsParams.jumpWallVelVertical);
                    }
                }
                else if (IsOnWallRight == true)
                {
                    if (currentVelocity.y <= 0)
                        currentVelocity = new Vector2(-PhysicsParams.jumpWallVelHorizontal, PhysicsParams.jumpWallVelVertical);
                    else
                        currentVelocity = new Vector2(-PhysicsParams.jumpWallVelHorizontal, currentVelocity.y + PhysicsParams.jumpWallVelVertical);
                }
            }
        }
        //did player lift the jump button?
        if (isKeyDownJump == false)
        {
            keyJumpPressed = false;
        }

        //let's apply force in case we are holding the jump key during a jump.
        if (keyJumpPressed == true)
        {
            SimAddForce(new Vector2(0, PhysicsParams.jumpUpForce) * EntityMass);
        }
        //however let's stop doing that as soon as we fall down after the up-phase.
        if (keyJumpPressed == true && currentVelocity.y <= 0)
        {
            keyJumpPressed = false;
        }

        //let's apply additional gravity in case we're in air moving up but not holding the jump button
        if (keyJumpPressed == false && isPlayerInAir == true && currentVelocity.y > 0)
        {
            SimAddForce(new Vector2(0, PhysicsParams.jumpGravity) * EntityMass);
        }

        //-----------------
        //IN AIR SIDEWAYS:
        if (isPlayerInAir == true)
        {
            //steering into moving direction (slow accel)
            if (isKeyDownLeft == true && currentVelocity.x <= 0)
                SimAddForce(new Vector2(-PhysicsParams.inAirMoveHorizontalForce, 0) * EntityMass);
            else if (isKeyDownRight == true && currentVelocity.x >= 0)
                SimAddForce(new Vector2(PhysicsParams.inAirMoveHorizontalForce, 0) * EntityMass);
            //steering against moving direction (fast reverse accel)
            else if (isKeyDownLeft == true && currentVelocity.x >= 0)
                SimAddForce(new Vector2(-PhysicsParams.inAirMoveHorizontalForceReverse, 0) * EntityMass);
            else if (isKeyDownRight == true && currentVelocity.x <= 0)
                SimAddForce(new Vector2(PhysicsParams.inAirMoveHorizontalForceReverse, 0) * EntityMass);
        }

        bool isMovingWithVelocity = false; // DICIDE IF PLAYER MOVING WITH VELOCITY OR AGIANTS VELOCITY ON GROUND
        //-----------------
        //ON GROUND SIDEWAYS:
        if (isPlayerInAir == false)
        {
            //steering into moving direction (slow accel)
            if (isKeyDownLeft == true && currentVelocity.x <= 0)
            {
                // MOVING LEFT WITH VELOCITY DIRECTION!
                SimAddForce(new Vector2(-PhysicsParams.onGroundMoveHorizontalForce, 0) * EntityMass);
                isMovingWithVelocity = true;
            }
            else if (isKeyDownRight == true && currentVelocity.x >= 0)
            {
                // MOVING RIGHT WITH VELOCITY DIRECTION!
                SimAddForce(new Vector2(PhysicsParams.onGroundMoveHorizontalForce, 0) * EntityMass);
                isMovingWithVelocity = true;
            }

            //steering against moving direction (fast reverse accel)
            else if (isKeyDownLeft == true && currentVelocity.x >= 0)
            {
                // MOVING LEFT AGAINTS VELOCITY DIRECTION (ACTUALY MOVING RIGHT)!
                SimAddForce(new Vector2(-PhysicsParams.onGroundMoveHorizontalForceReverse, 0) * EntityMass);
                isMovingWithVelocity = false;
            }
            else if (isKeyDownRight == true && currentVelocity.x <= 0)
            {
                // MOVING LEFT AGAINTS VELOCITY DIRECTION (ACTUALY MOVING LEFT)!
                SimAddForce(new Vector2(PhysicsParams.onGroundMoveHorizontalForceReverse, 0) * EntityMass);
                isMovingWithVelocity = false;
            }
            

            //not steering -> brake due to friction.
            else if (isKeyDownLeft != true && isKeyDownRight != true && currentVelocity.x > 0)
            {
                SimAddForce(new Vector2(-PhysicsParams.groundFriction, 0) * EntityMass);
                isMovingWithVelocity = false;
            }
            else if (isKeyDownLeft != true && isKeyDownRight != true && currentVelocity.x < 0)
            {
                SimAddForce(new Vector2(PhysicsParams.groundFriction, 0) * EntityMass);
                isMovingWithVelocity = false;
            }

            //in case the velocity is close to 0 and no keys are pressed we should make the the player stop.
            //to do this let's first undo the prior friction force, and then set the velocity to 0.
            if (isKeyDownLeft != true && isKeyDownRight != true && currentVelocity.x > 0 && currentVelocity.x < PhysicsParams.groundFrictionEpsilon)
            {
                SimAddForce(new Vector2(PhysicsParams.groundFriction, 0) * EntityMass);
                currentVelocity.x = 0;
            }
            else if (isKeyDownLeft != true && isKeyDownRight != true && currentVelocity.x < 0 && currentVelocity.x > -PhysicsParams.groundFrictionEpsilon)
            {
                SimAddForce(new Vector2(-PhysicsParams.groundFriction, 0) * EntityMass);
                currentVelocity.x = 0;
            }
        }

        // RIGHT AND LEFt FACING DIRECTIONS
        if(!IsOnWall)
        {
            if (currentVelocity.x > 0)
                SetDirectionRight(true);
            else if (currentVelocity.x < 0)
                SetDirectionRight(false);
        } else
        {
            if (IsOnWallRight)
                SetDirectionRight(false);
            else
                SetDirectionRight(true);
        }
        

        // DEALING WITH ANIMATIONS
        string currentState = NinjaState.Idle;

        // JUMPING ANIMATION
        if (isPlayerInAir && !isPlayerOnWall)
        {
            currentState = NinjaState.Jump;
        }

        // JUMPING ANIMATION
        if (isPlayerOnWall)
        {
            currentState = NinjaState.OnWall;
        }

        // IDLE, WALKING AND STOP WALKING ANIMATIONS
        if (!isPlayerInAir)
        {
            if (currentVelocity.x < PhysicsParams.groundFrictionEpsilon && currentVelocity.x > -PhysicsParams.groundFrictionEpsilon)
            {
                currentState = NinjaState.Idle;
            }
            else
            {
                if (isMovingWithVelocity)
                    currentState = NinjaState.Walk;
                else
                    currentState = NinjaState.StopWalk;
            }
        }

        if (currentShurikenCooldown > 0)
        {
            currentShurikenCooldown -= Time.deltaTime;
            if (currentShurikenCooldown < 0)
                currentShurikenCooldown = 0;
        }

        if (isKeyDownThrow && IsOnGround && CanThrowShuriken())
        {
            currentState = NinjaState.Throw;
            currentShurikenCooldown = shurikenCooldown;
            ShurikenController sc = Instantiate(shuriken, transform.position, transform.rotation);
            sc.speed = shurikenSpeed * ((transform.localRotation == Quaternion.Euler(0, 0, 0)) ? 1 : -1);
            sc.tag = "Shuriken";
            audioSrc.PlayOneShot(throwShurikenSound, volume);
        }

        if (currentState != lastState)
        {
            SetAnimationProps(currentState); // IMPORTANT!!! CHANGE ANIMATION ONLY WHEN NECCESSARY!
            lastState = currentState;
        }
            
        

    }

    public void ResetVelocity()
    {
        currentVelocity = Vector2.zero;
    }

    public bool CanThrowShuriken()
    {
        return currentShurikenCooldown == 0;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {

        foreach (ContactPoint2D contactPoint in collision.contacts)
        {
            if (GetIsVectorClose(new Vector2(0, 1), contactPoint.normal))
            {
                timeRealLastGroundCollision = Time.realtimeSinceStartup;
                currentVelocity.y = Mathf.Clamp(currentVelocity.y, 0, Mathf.Abs(currentVelocity.y));
            }
            if (GetIsVectorClose(new Vector2(1, 0), contactPoint.normal))
            {
                timeRealLastWallLeftCollision = Time.realtimeSinceStartup;
                currentVelocity.x = Mathf.Clamp(currentVelocity.x, 0, Mathf.Abs(currentVelocity.x));
            }
            if (GetIsVectorClose(new Vector2(-1, 0), contactPoint.normal))
            {
                timeRealLastWallRightCollision = Time.realtimeSinceStartup;
                currentVelocity.x = Mathf.Clamp(currentVelocity.x, -Mathf.Abs(currentVelocity.x), 0);
            }
            if (GetIsVectorClose(Vector2.down, contactPoint.normal))
            {
                currentVelocity.y = Mathf.Clamp(currentVelocity.y, -Mathf.Abs(currentVelocity.y), 0);
            }
        }
    }

    private bool GetIsVectorClose(Vector2 vectorA, Vector2 vectorB)
    {
        return (Mathf.Approximately(0, Vector2.Distance(vectorA, vectorB)));
    }

    public void OnLifeChanged(int life, Vector2 contactVector)
    {
        const float forceEnemyCollision = 15.0f;
        currentVelocity = contactVector.normalized * forceEnemyCollision;
    }

    public void ResetPlayer()
    {
        currentVelocity = Vector2.zero;
    }

    public void SetAnimationProps(string state)
    {
        animator.SetBool(NinjaState.Idle, false);
        animator.SetBool(NinjaState.Walk, false);
        animator.SetBool(NinjaState.Jump, false);
        animator.SetBool(NinjaState.OnWall, false);
        animator.SetBool(NinjaState.StopWalk, false);
        animator.SetBool(NinjaState.Throw, false);

        animator.SetBool(state, true);
    }

    public void SetDirectionRight(bool isRight)
    {
        if (isRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
