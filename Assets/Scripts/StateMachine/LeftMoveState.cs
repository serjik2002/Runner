using System.Collections;
using UnityEngine;

public class LeftMoveState : PlayerState
{
    private PlayerController player;
    private readonly string _moveLeft = "MoveLeft";
    public LeftMoveState(PlayerController player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        Debug.Log("Enter LeftMove state");
        PlayAnimation(_moveLeft, player);
    }

    public override void Exit()
    {
        Debug.Log("Exit LeftMove state");
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator MoveHorizontal(Vector3 direction)
    {
        Vector3 targetPosition = player.transform.position;


        Line newLine = GetNewLine(direction);

        if (newLine != player.CurrentLine)
        {
            player.ChangeLine(newLine);
            float positionValue = player.Position[player.CurrentLine];
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                targetPosition = new Vector3(positionValue, player.transform.position.y, player.transform.position.z);
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, player.LineChangeSpeed * Time.deltaTime);
                yield return null;
            }
            player.transform.position = targetPosition;
        }
    }

    private Line GetNewLine(Vector3 direction)
    {
        if (direction == Vector3.left)
        {
            return (Line)Mathf.Clamp((int)player.CurrentLine - 1, 0, 2);
        }
        else if (direction == Vector3.right)
        {
            return (Line)Mathf.Clamp((int)player.CurrentLine + 1, 0, 2);
        }
        else
        {
            return player.CurrentLine;
        }
    }
}
