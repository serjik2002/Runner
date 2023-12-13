using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Unity.VisualScripting;
using System;

public enum Line
{
    Left, Middle, Right
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _lineStep = 2.5f;
    [SerializeField] private Line _currentLine;
    [SerializeField] private int _jumpForce = 5;
    [SerializeField] private PlatformInputConroller[] _inputs;

    private PlatformInputConroller _currentInput;
    private Rigidbody _rigidBody;

    public UnityAction OnPlayerDie;

    private void Start()
    {
        _currentInput = _inputs.FirstOrDefault(input => input.CheckPlatform() != null);
        _rigidBody = GetComponent<Rigidbody>();
        _currentLine = Line.Middle;
        SwipeController.Instance.MoveEvent += MovePlayer;
    }


    private void Update()
    {
        Move(_currentInput);
    }

    public void Move(PlatformInputConroller controller)
    {
        
        Direction direction = controller.PerformControl();
        switch (direction)
        {
            case Direction.None:
                return;
                break;
            case Direction.Up:
                Jump();
                break;
            case Direction.Down:
                MoveDown();
                break;
            case Direction.Left:
                MoveHorizontal(Vector3.left);
                break;
            case Direction.Right:
                MoveHorizontal(Vector3.right);
                break;
            default:
                break;
        }
    }

    private void MoveDown()
    {
        throw new NotImplementedException();
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

    public void Jump()
    {
        _rigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Die")
        {
            OnPlayerDie?.Invoke();
        }
    }
}
