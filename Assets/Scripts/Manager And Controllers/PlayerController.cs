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
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _lineStep = 2.5f;
    [SerializeField] private Line _currentLine;
    [SerializeField] private int _jumpForce = 5;
    [SerializeField] private int _gravityForse = 5;
    [SerializeField] private int _lineChangeSpeed = 10;
    [SerializeField] private float _timeToStopMoveDown = 0.2f;
    [SerializeField] private PlatformInputConroller[] _inputs;

    private PlatformInputConroller _currentInput;
    private Rigidbody _rigidBody;
    private bool _isGrounded;
    private Dictionary<Line, float> _position = new Dictionary<Line, float>();

    public UnityAction OnPlayerDie;

    private void Start()
    {
        _currentInput = _inputs.FirstOrDefault(input => input.CheckPlatform() != null);
        _rigidBody = GetComponent<Rigidbody>();
        _currentLine = Line.Middle;
        InitPositionDictionary();
        _isGrounded = true;
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
        SwipeManager.Direction direction = controller.PerformControl();        
        switch (direction)
        {
            case SwipeManager.Direction.Up:
                
                Jump();
                break;
            case SwipeManager.Direction.Down:
                MoveDown();
                break;
            case SwipeManager.Direction.Left:
                StopAllCoroutines();
                StartCoroutine(MoveHorizontal(Vector3.left));
                break;
            case SwipeManager.Direction.Right:
                StopAllCoroutines();
                StartCoroutine(MoveHorizontal(Vector3.right));
                break;
        }
    }

    private void MoveDown()
    {
        if(!_isGrounded)
        {
            _rigidBody.velocity = Vector3.down * _jumpForce;
            transform.localScale = new Vector3(1, 0.5f, 1);
            Invoke("ReturnScale", _timeToStopMoveDown);
           
        }
        else
        {
            transform.localScale = new Vector3(1, 0.5f, 1);
            Invoke("ReturnScale", _timeToStopMoveDown);
        }
       
    }
    private void ReturnScale()
    {
        transform.localScale = Vector3.one;
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

    void OnCollisionStay(Collision collision)
    {
        // Проверяем, столкнулся ли персонаж с объектом, имеющим коллайдер

        _isGrounded = true;
        
    }

    void OnCollisionExit(Collision collision)
    {
        // Проверяем, перестал ли персонаж сталкиваться с объектом

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
