using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : IState
{
    PlayerController player;

    PlayerInput playerInput;

    public PlayerMoveState(PlayerController player)
    {
        this.player = player;
        playerInput = player.playerInput;
    }
    public void OnEnter()
    {
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
        PlayerMovement();

        PlayerRotation();
    }

    public void OnUpdate()
    {
        player.animator.SetFloat("MoveSpeed", player.curSpeed);

        if (playerInput.isJump)
        {
            player.TranslateState(PlayerStateType.Jump);
        }

        if (playerInput.isRoll)
        {
            player.TranslateState(PlayerStateType.Roll);
        }

        //if (playerInput.isMeleeAttack)
        //{
        //    player.TranslateState(PlayerStateType.MeleeAttack);
        //}
    }

    void PlayerMovement()
    {
        player.playerMove = new Vector3(playerInput.moveInput.x, 0, playerInput.moveInput.y);
        if (playerInput.moveInput.sqrMagnitude > 0.1f)
        {
            player.isMove = true;
            player.curSpeed = Mathf.MoveTowards(player.curSpeed, player.maxSpeed, player.acceleratedSpeed * Time.deltaTime);
        }
        else if (playerInput.moveInput.sqrMagnitude < 0.1f)
        {
            if(player.curSpeed > 0)
            {
                player.curSpeed -= player.deacceleratedSpeed * Time.deltaTime;
            }
            else
            {
                player.isMove = false;
            }
            
        }

        player.playerMove *=  player.curSpeed * Time.deltaTime;

        player.playerMove = player.renderCamera.TransformDirection(player.playerMove);
        player.playerMove.y = 0;

        //player.controller.Move(player.playerMove);
        player.isGrounded = Physics.CheckSphere(player.GroundDetector.transform.position, player.groundDetectorRadius, LayerMask.NameToLayer("Enviroment"));
    }

    void PlayerRotation()
    {
        if (playerInput.moveInput.sqrMagnitude < 0.1f)
        {
            player.curRotateSpeed = Mathf.MoveTowards(player.curRotateSpeed, player.maxRotateSpeed, player.acceleratedRotateSpeed);
        }
        else
        {
            player.curRotateSpeed = 0;
        }

        if (player.playerMove.x != 0 || player.playerMove.z != 0)
        {
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, Quaternion.LookRotation(player.playerMove), player.curRotateSpeed * Time.deltaTime);
        }

    }
}
