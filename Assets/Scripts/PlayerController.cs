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

    private void Start()
    {
        _currentLine = Line.Middle;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            
            MoveHorizontal(Vector3.left);
            
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            
            MoveHorizontal(Vector3.right);
            
        }
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
