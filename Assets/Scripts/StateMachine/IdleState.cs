using UnityEngine;

public class IdleState : PlayerState
{
    private PlayerController player;
    private readonly string _idle = "Idle";
    public IdleState(PlayerController player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        Debug.Log("Enter idle state");
        PlayAnimation(_idle, player);
    }

    public override void Exit()
    {
        Debug.Log("Exit idle state");
        //player.PlayerAnimator.stop
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}

public interface IAnimationStatePlay
{
    public void PlayAnimation(string animationName, PlayerController player);
}
