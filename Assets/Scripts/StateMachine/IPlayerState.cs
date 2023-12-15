using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void Enter();
    public void Update();
    public void Exit();
}
