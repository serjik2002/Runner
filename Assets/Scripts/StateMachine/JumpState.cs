using UnityEngine;

public class JumpState : PlayerState
{
    private PlayerController player;
    private readonly string _jump = "Jump";
    public JumpState(PlayerController player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        Debug.Log("Enter Jump state");
        PlayAnimation(_jump, player);
        Jump();
    }

    public override void Exit()
    {
        Debug.Log("Exit Jump state");
        //player.PlayerAnimator.ResetTrigger(_jump);
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public void Jump()
    {
        if (!player.IsGrounded)
            return;
        player.transform.localScale = Vector3.one;
        player.RigidBody.velocity = Vector3.up * player.JumpForce;
        Physics.gravity = Vector3.down * player.GravityForce;
    }

}




