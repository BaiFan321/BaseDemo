using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : IState
{
    PlayerController player;
    PlayerInput playerInput;


    public PlayerJumpState(PlayerController player)
    {
        this.player = player;
        playerInput = player.playerInput;
    }
    public void OnEnter()
    {
        player.animator.Play("PlayerJump");
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        PlayerVerticalMove();
    }

    public void OnUpdate()
    {
        player.animator.SetBool("isJump", playerInput.isJump);

        if(!playerInput.isJump && player.isGrounded)
        {
            player.TranslateState(PlayerStateType.Move);
        }
        
    }

    public void PlayerVerticalMove()
    {

        if (player.isGrounded ) 
        {
            player.VerticalSpeed = player.GravitySpeed * player.stickProportion;
            if (playerInput.isJump)
            {
                player.VerticalSpeed = player.GravitySpeed;
                player.isGrounded = false;
            }
            Debug.Log("isGrounded: player.VerticalSpeed " + player.VerticalSpeed);

        }
        else
        {
            player.VerticalSpeed -= player.GravitySpeed * Time.deltaTime;
            player.playerMove.y = player.VerticalSpeed * Time.deltaTime;
            Debug.Log("NotGrounded: player.VerticalSpeed " + player.VerticalSpeed + "  playerInput.isJump:" + playerInput.isJump);
            //player.controller.Move(player.playerMove);
            player.isGrounded = Physics.CheckSphere(player.GroundDetector.transform.position, player.groundDetectorRadius, LayerMask.GetMask("Enviroment"));
            Debug.Log("isGrounded :" + player.isGrounded + "      player.GroundDetector.position" + player.GroundDetector.transform.position.y);
            if(player.isGrounded && player.VerticalSpeed > 0)
            {
                player.isGrounded = false;
            }
        }
    }
}
