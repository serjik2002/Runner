using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Unity.VisualScripting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum Line
{
    Left, Middle, Right
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _lineStep = 2.5f;
    [SerializeField] private Line _currentLine;
    [SerializeField] private int _jumpForce = 5;
    [SerializeField] private int _gravityForse = 5;
    [SerializeField] private int _lineChangeSpeed = 10;
    [SerializeField] private float _timeToStopRoll = 0.2f;
    [SerializeField] private float _angle = 45;
    [SerializeField] private PlatformInputConroller[] _inputs;
    [SerializeField] private PanelStartClick _panelStartClick;

    private PlatformInputConroller _currentInput;
    private Rigidbody _rigidBody;
    private Dictionary<Line, float> _position = new Dictionary<Line, float>();
    private StateMachine _stateMachine = new StateMachine();
    private bool _isGrounded;
    private bool _isRunning = false;
    private Animator _animator;
    private CapsuleCollider _collider;

    public bool IsGrounded => _isGrounded;
    public bool IsRunning => _isRunning;
    public int JumpForce => _jumpForce;
    public int GravityForce => _gravityForse;
    public float TimeToStopRoll => _timeToStopRoll;
    public float LineChangeSpeed => _lineChangeSpeed;
    public Animator PlayerAnimator => _animator;
    public Rigidbody RigidBody => _rigidBody;
    public CapsuleCollider PlayerCollider => _collider;
    public Line CurrentLine => _currentLine;

    public Dictionary<Line, float> Position => _position;

    public UnityAction OnPlayerDie;

    private void Start()
    {
        _currentInput = _inputs.FirstOrDefault(input => input.CheckPlatform() != null);
        _rigidBody = GetComponent<Rigidbody>();
        _currentLine = Line.Middle;
        InitPositionDictionary();
        _isGrounded = true;
        _animator = GetComponentInChildren<Animator>();
        _stateMachine.Initialize(new IdleState(this));
        _collider = GetComponent<CapsuleCollider>();
        _panelStartClick.OnClickStartGame.AddListener(StartRun);
    }


    private void Update()
    {
        if(_isRunning)
        {
            Move(_currentInput);
        }
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
        SwipeManager.Direction direction = controller.PerformControl();        
        switch (direction)
        {
            case SwipeManager.Direction.Up:
                _stateMachine.ChangeState(new JumpState(this));     
                break;
            case SwipeManager.Direction.Down:
                _stateMachine.ChangeState(new RollDownState(this));
                Invoke("ReturnColiderHeight", _timeToStopRoll);
                break;
            case SwipeManager.Direction.Left:
                StopAllCoroutines();
                _stateMachine.ChangeState(new HorizontalMoveState(this, -_angle));
                StartCoroutine(MoveHorizontal(Vector3.left));
                Invoke("ReturnRotation", _timeToStopRoll);
                break;
            case SwipeManager.Direction.Right:
                StopAllCoroutines();
                _stateMachine.ChangeState(new HorizontalMoveState(this, _angle));
                StartCoroutine(MoveHorizontal(Vector3.right));
                Invoke("ReturnRotation", _timeToStopRoll);
                break;
            case SwipeManager.Direction.None:
                return;
            
        }
    }

    private void ReturnColiderHeight()
    {
        _collider.height = 2f;
    }
    
    private void ReturnRotation()
    {
        transform.eulerAngles = Vector3.zero;
    }

    public void Jump()
    {
        if (!_isGrounded)
            return;
        transform.localScale = Vector3.one;
        _rigidBody.velocity = Vector3.up * _jumpForce;
        Physics.gravity = Vector3.down * _gravityForse;
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
            for (float i = 0; i < 1; i+= Time.deltaTime)
            {
                targetPosition = new Vector3(positionValue, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _lineChangeSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition;
        }
    }

    public void ChangeLine(Line newLine)
    {
        _currentLine = newLine;
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

    private void StartRun()
    {
        _stateMachine.ChangeState(new RunState(this));
        _isRunning = true;
    }

    void OnCollisionStay(Collision collision)
    {
        _isGrounded = true;  
    }

    void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Die")
        {
            OnPlayerDie?.Invoke();
            SceneManager.LoadScene("GameScene");
        }
    }
}
