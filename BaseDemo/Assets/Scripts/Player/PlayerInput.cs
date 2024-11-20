using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private static PlayerInput instance;
    public static PlayerInput Instance
    {
        get
        {
            return instance;
        }
    }

    protected Vector2 m_moveInput;
    protected Vector2 m_mouseInput;
    protected bool m_isJump;
    protected bool m_isRoll;
    protected bool m_isMeleeAttack;
    

    public Vector2 MoveInput => m_moveInput;

    public Vector2 MouseInput => m_mouseInput;

    public bool isJump { get { return m_isJump; } }

    public bool isRoll { get { return m_isRoll; } }

    public bool isMeleeAttack { get { return m_isMeleeAttack; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            throw new UnityException("No more than one playerInput!!!");
        }
        m_moveInput = new Vector2(0, 0);
        m_mouseInput = new Vector2(0, 0);
    }

    void Update()
    {
        m_moveInput.x = Input.GetAxis("Horizontal");
        m_moveInput.y = Input.GetAxis("Vertical");

        m_mouseInput.x = Input.GetAxis("Mouse X");
        m_mouseInput.y = Input.GetAxis("Mouse Y");

        m_isJump = Input.GetButton("Jump");

        m_isRoll = Input.GetKeyDown(KeyCode.C);

        m_isMeleeAttack = Input.GetButtonDown("Fire1");

        
    }
}
