using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    protected static PlayerInput s_Instance;
    public static PlayerInput playerInput { get { return s_Instance; } }

    protected Vector2 m_moveInput;
    protected bool m_isJump;
    protected bool m_isRoll;
    protected bool m_isMeleeAttack;

    public Vector2 moveInput { get { return m_moveInput; } }

    public bool isJump { get { return m_isJump; } }

    public bool isRoll { get { return m_isRoll; } }

    public bool isMeleeAttack { get { return m_isMeleeAttack; } }

    private void Awake()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
        }else if(s_Instance != this)
        {
            throw new UnityException("No more than one playerInput!!!");
        }
    }

    void Update()
    {
        m_moveInput.x = Input.GetAxis("Horizontal");
        m_moveInput.y = Input.GetAxis("Vertical");

        m_isJump = Input.GetButton("Jump");

        m_isRoll = Input.GetKeyDown(KeyCode.C);

        m_isMeleeAttack = Input.GetButtonDown("Fire1");
    }
}
