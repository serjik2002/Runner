using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Unity.VisualScripting;
using System;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] private int _lineChangeSpeed = 10;
    [SerializeField] private PlatformInputConroller[] _inputs;

    private PlatformInputConroller _currentInput;
    private Rigidbody _rigidBody;
    private Dictionary<Line, float> _position = new Dictionary<Line, float>();

    public UnityAction OnPlayerDie;

    private void Start()
    {
        _currentInput = _inputs.FirstOrDefault(input => input.CheckPlatform() != null);
        _rigidBody = GetComponent<Rigidbody>();
        _currentLine = Line.Middle;
        InitPositionDictionary();
        //SwipeController.Instance.MoveEvent.MovePlayer;
    }


    private void Update()
    {
        Move(_currentInput);
    }

    private void InitPositionDictionary()
    {
        float position = -_lineStep;
        for (int i = 0; i < 3; i++)
        {
            _position.Add((Line)i, position);
            position += _lineStep;
        }

            
    }

    public void Move(PlatformInputConroller controller)
    {
        //останавливать корутину когда получаешь инпут
        SwipeManager.Direction direction = controller.PerformControl();
        switch (direction)
        {
            case SwipeManager.Direction.None:
                return;
                break;
            case SwipeManager.Direction.Up:
                Jump();
                break;
            case SwipeManager.Direction.Down:
                MoveDown();
                break;
            case SwipeManager.Direction.Left:
                StartCoroutine(MoveHorizontal(Vector3.left));
                break;
            case SwipeManager.Direction.Right:
                StartCoroutine(MoveHorizontal(Vector3.right));
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

    public IEnumerator MoveHorizontal(Vector3 direction)
    {
        Vector3 targetPosition = transform.position;
        Vector3 prevPosition = transform.position;
        

        Line newLine = GetNewLine(direction);

        if (newLine != _currentLine)
        {
            _currentLine = newLine;
            float positionValue = _position[_currentLine];
            targetPosition = new Vector3(positionValue, transform.position.y, transform.position.z);
            for (float i = 0; i < 1; i+= Time.deltaTime)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _lineChangeSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition;
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
