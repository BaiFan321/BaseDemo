using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MonsterController : MonoBehaviour
{
    private static MonsterController instance;
    public MonsterController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = gameObject.AddComponent<MonsterController>();
            }
            return instance;
        }
    }

    private CharacterController controller;

    private PlayerInput playerInput;

    private float maxMoveSpeed = 3.0f;
    private float curMoveSpeed = 0;
    private float accelerateMoveSpeed = 4.0f;

    private float maxRotateSpeed = 600f;
    private float curRotateSpeed = 100f;
    private float accelerateRotateSpeed = 1500f;

    private Quaternion targetRotation;
    private Vector3 targetMove;

    public CinemachineFreeLook cf;

    private Animator aniController;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        if (!controller)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        playerInput = gameObject.GetComponent<PlayerInput>();
        if (!playerInput)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }

        aniController = gameObject.GetComponent<Animator>();
        if (!aniController)
        {
            aniController = gameObject.AddComponent<Animator>();
        }

    }

    private void Update()
    {
        MonsterMove();

        MonsterRotate();
    }

    private void MonsterMove()
    {
        Vector3 moveDir = new Vector3(PlayerInput.Instance.MoveInput.x, 0, PlayerInput.Instance.MoveInput.y);
        Vector3 curMoveDir = moveDir.normalized;
        curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, maxMoveSpeed, accelerateMoveSpeed * Time.deltaTime);
        if (curMoveDir.magnitude < 0.1f)
        {
            curMoveSpeed = 0f;
        }


        //targetMove = new Vector3(cameraDir.x * curMoveDir.x, 0f, cameraDir.z * curMoveDir.z);
        //targetMove = cfRight * curMoveDir.x + cfForward * curMoveDir.z;


        aniController.SetFloat("MoveSpeed", curMoveSpeed);

        controller.Move(curMoveSpeed * transform.forward * Time.deltaTime);
    }

    private void MonsterRotate()
    {
        Vector3 moveDir = new Vector3(PlayerInput.Instance.MoveInput.x, 0, PlayerInput.Instance.MoveInput.y);
        Vector3 curRotation = moveDir.normalized;
        curRotateSpeed = Mathf.MoveTowards(curRotateSpeed, maxRotateSpeed, accelerateRotateSpeed * Time.deltaTime);
        if(curRotation.magnitude < 0.1f)
        {
            curRotateSpeed = 0f;
        }

        Vector3 forward = Quaternion.Euler(0, cf.m_XAxis.Value, 0) * Vector3.forward;
        forward.y = 0f;
        forward.Normalize();

        if(Mathf.Approximately(Vector3.Dot(curRotation, Vector3.forward), -1.0f)){
            targetRotation = Quaternion.LookRotation(-forward);
        }
        else
        {
            Quaternion turnRotation = Quaternion.FromToRotation(Vector3.forward, curRotation);
            targetRotation = Quaternion.LookRotation(turnRotation * forward);
        }  
        targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, curRotateSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
        Debug.Log("transform.forward = " + transform.forward + "  transform.rotation = " + transform.rotation + "forward = " + forward);
        
        //Quaternion curDir = Quaternion.FromToRotation(transform.forward, moveDir);
        //transform.forward = curDir * transform.forward;
        //Quaternion curDir = Quaternion.LookRotation(moveDir, Vector3.up);
        //Quaternion lerpDir = Quaternion.Lerp(transform.localRotation, curDir, accelerateRotateSpeed * Time.deltaTime); ;
        //transform.rotation = lerpDir;
        //cv.LookAt.localEulerAngles = moveDir;

    }
}
