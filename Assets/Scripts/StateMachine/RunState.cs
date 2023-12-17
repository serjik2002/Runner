using UnityEngine;

public class RunState : PlayerState
{
    private PlayerController player;
    private readonly string _run = "Run";
    public RunState(PlayerController player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        Debug.Log("Enter run state");
        PlayAnimation(_run, player);
    }

    public override void Exit()
    {
        Debug.Log("Exit run state");
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
