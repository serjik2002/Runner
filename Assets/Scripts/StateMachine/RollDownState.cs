using UnityEngine;

public class RollDownState : PlayerState
{
    private PlayerController player;
    private readonly string _rollDown = "RollDown";
    public RollDownState(PlayerController player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        Debug.Log("Enter RollDown state");
        PlayAnimation(_rollDown, player);
        RollDown();
    }

    public override void Exit()
    {
        Debug.Log("Exit RollDown state");
        player.PlayerAnimator.ResetTrigger(_rollDown);
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
    private void RollDown()
    {
        var heigh = player.PlayerCollider.height;
        if (!player.IsGrounded)
        {
            player.RigidBody.velocity = Vector3.down * player.JumpForce;
            //player.transform.localScale = new Vector3(1, 0.5f, 1);
            player.PlayerCollider.height /= heigh;
        }
        else
        {
            player.PlayerCollider.height /= heigh;
        }

    }
}