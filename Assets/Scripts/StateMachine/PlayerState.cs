using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : IAnimationStatePlay
{
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

    public void PlayAnimation(string animationName, PlayerController player)
    {
        var animHash = Animator.StringToHash(animationName);
        player.PlayerAnimator.CrossFade(animHash, 0f);
    }
}
