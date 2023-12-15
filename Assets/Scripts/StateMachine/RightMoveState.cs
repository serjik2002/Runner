using UnityEngine;

public class RightMoveState : PlayerState
{
    private PlayerController player;
    private readonly string _moveRight = "MoveRight";
    public RightMoveState(PlayerController player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        Debug.Log("Enter RightMove state");
        PlayAnimation(_moveRight, player);
    }

    public override void Exit()
    {
        Debug.Log("Exit RightMove state");
        player.PlayerAnimator.ResetTrigger(_moveRight);
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}




