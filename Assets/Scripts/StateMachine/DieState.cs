using UnityEngine;

public class DieState : PlayerState
{
    private PlayerController player;
    private readonly string _die = "Die";
    public DieState(PlayerController player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        Debug.Log("Enter RightMove state");
        PlayAnimation(_die, player);
    }

    public override void Exit()
    {
        Debug.Log("Exit RightMove state");
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}




