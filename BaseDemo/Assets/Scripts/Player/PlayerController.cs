using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle, Move, Jump, Roll, MeleeAttack, Hurt, Die
}

public class PlayerController : MonoBehaviour
{
    protected static PlayerController s_Instance;
    public static PlayerController playerController { get { return s_Instance; } }

    #region Properties
    public PlayerInput playerInput;
    public CharacterController controller;
    public Animator animator;
    public Rigidbody rb;
    public Transform GroundDetector;
    public float groundDetectorRadius = 0.1f;

    public Vector3 playerMove;

    public Transform renderCamera;

    //Move
    public float maxSpeed = 5f;
    public float curSpeed;
    public float acceleratedSpeed = 0.1f;
    public float deacceleratedSpeed = 20f;
    public bool isMove;

    //Rotate
    public float maxRotateSpeed = 1200f;
    public float minRotateSpeed = 400f;
    public float curRotateSpeed;
    public float acceleratedRotateSpeed = 20f;

    //Jump
    public float VerticalSpeed;
    public float GravitySpeed = 10f;
    public bool isGrounded = true;
    public bool isReadyToJump = true;
    public float stickProportion = 0.3f;


    //Attack
    public bool isMeleeAttack;


    //Roll
    public bool isRoll;


    public bool isHurt;
    public bool isDie;


    private IState curState;


    private Dictionary<PlayerStateType, IState> states = new Dictionary<PlayerStateType, IState>();
    #endregion


    void Start()
    {
        s_Instance = this;

        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        states.Add(PlayerStateType.Idle, new PlayerIdleState(this));
        states.Add(PlayerStateType.Move, new PlayerMoveState(this));
        states.Add(PlayerStateType.Jump, new PlayerJumpState(this));
        states.Add(PlayerStateType.Roll, new PlayerRollState(this));
        states.Add(PlayerStateType.MeleeAttack, new PlayerMeleeAttackState(this));
        states.Add(PlayerStateType.Hurt, new PlayerHurtState(this));
        states.Add(PlayerStateType.Die, new PlayerDieState(this));

        curState = states[PlayerStateType.Idle];

    }

    public void TranslateState(PlayerStateType type)
    {
        if(curState != null)
        {
            curState.OnExit();
        }
        curState = states[type];
        curState.OnEnter();
    }

    void FixedUpdate()
    {
        curState.OnFixedUpdate();
    }

    private void Update()
    {
        curState.OnUpdate();
    }


}
