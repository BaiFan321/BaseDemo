using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState
{
    PlayerController player;
    PlayerInput playerInput;

    public PlayerIdleState(PlayerController player)
    {
        this.player = player;
        this.playerInput = player.playerInput;
    }
    public void OnEnter()
    {
        player.animator.Play("PlayerIdle");
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        if (playerInput.MoveInput != Vector2.zero)
        {
            player.TranslateState(PlayerStateType.Move);
        }

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
}
