using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    //public Transform targetPoint;

    //public MonsterController monster;

    private PlayerInput playerInput;

    //private float cameraSensitivity = 100f;

    //private int xRotateMin = -90;
    //private int xRotateMax = 90;

    private CinemachineFreeLook cf;

    //private int yRotateMin = -30;
    //private int yRotateMax = 90;

    //private float xRotation;
    //private float yRotation;

    private void Awake()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        //if (!playerInput)
        //{
        //    playerInput = gameObject.AddComponent<PlayerInput>();
        //}
        cf = GetComponent<CinemachineFreeLook>();
    }
    private void Update()
    {
        CameraRotate();
    }

    private void CameraRotate()
    {
        float x = PlayerInput.Instance.MouseInput.x;
        float y = PlayerInput.Instance.MouseInput.y;

        //xRotation -= x * cameraSensitivity * Time.deltaTime;
        //yRotation += y * cameraSensitivity * Time.deltaTime;

        //yRotation = Mathf.Clamp(yRotation, yRotateMin, yRotateMax);

        //Quaternion curRotation = Quaternion.Euler(-yRotation, -xRotation, 0);
        //transform.rotation = curRotation;
        //Debug.Log(transform.eulerAngles);

        cf.m_XAxis.m_InputAxisValue = x;
        cf.m_YAxis.m_InputAxisValue = y;
    }
}
