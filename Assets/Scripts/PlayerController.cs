using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Line
{
    Left, Middle, Right
}

public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private float _lineStep = 2.5f;
    [SerializeField] private Line _currentLine;

    private SwipeController _swipeController;

    private void Start()
    {
        _currentLine = Line.Middle;
        _swipeController = FindObjectOfType<SwipeController>();
        _swipeController.MoveEvent += MovePlayer;
    }

    private void Update()
    {
        
    }

    public void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeController.Direction.Left])
        {

            MoveHorizontal(Vector3.left);

        }
        if (swipes[(int)SwipeController.Direction.Right])
        {

            MoveHorizontal(Vector3.right);

        }
        if (swipes[(int)SwipeController.Direction.Up])
        {

            Jump();

        }
    }

    private void Jump()
    {
        //jump
    }

    public void MoveHorizontal(Vector3 direction)
    {

        Line newLine = GetNewLine(direction);
        if(newLine != _currentLine)
        {
            _currentLine = newLine;
            transform.position += direction * _lineStep;

        }

        
    }

    private Line GetNewLine(Vector3 direction)
    {
        if (direction == Vector3.left)
        {
            return (Line)Mathf.Clamp((int)_currentLine - 1, 0, 2);
        }
        else if (direction == Vector3.right)
        {
            return (Line)Mathf.Clamp((int)_currentLine + 1, 0, 2);
        }
        else
        {
            return _currentLine;
        }
    }
}
