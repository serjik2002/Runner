using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { None, Up, Down, Left, Right, }
public class SwipeController : MonoBehaviour
{
    public static SwipeController Instance;

    private Vector2 startTouch;
    private bool touchMoved;
    private Vector2 swipeDelta;
    private readonly float SWIPE_TRESHOLD;
    private bool[] swipe = new bool[4];


    public enum Direction { Up, Down, Left, Right }
    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Vector2 GetTouchPosition()
    {
        return (Vector2)Input.mousePosition;
    }
    
    private bool TouchBegan()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool TouchEnded()
    {
        return Input.GetMouseButtonUp(0);
    }

    private bool GetTouch()
    {
        return Input.GetMouseButton(0);
    }
    private void Update()
    {
        if(TouchBegan())
        {
            startTouch = GetTouchPosition();
            touchMoved = true;
        }
        else if(TouchEnded() && touchMoved)
        {
            SendSwipe();
            touchMoved = false;
        }
        swipeDelta = Vector2.zero;
        if (touchMoved&&GetTouch())
        {
            swipeDelta = GetTouchPosition() - startTouch;
        }

        if (swipeDelta.magnitude> SWIPE_TRESHOLD)
        {
            if(Mathf.Abs(swipeDelta.x)> Mathf.Abs(swipeDelta.y))
            {
                swipe[(int)Direction.Left] = swipeDelta.x < 0;
                swipe[(int)Direction.Right] = swipeDelta.x > 0;
            }
            else
            {
                swipe[(int)Direction.Up] = swipeDelta.y > 0;
                swipe[(int)Direction.Down] = swipeDelta.y < 0;
            }
            SendSwipe();
        }
    }

    private void SendSwipe()
    {
        if(swipe[0]|| swipe[1]|| swipe[2]|| swipe[3])
        {
            MoveEvent?.Invoke(swipe);
        }

        Reset();
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        touchMoved = false;
        for (int i = 0; i < swipe.Length; i++)
        {
            swipe[i] = false;
        }
    }
}
