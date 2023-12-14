using Firebase.Auth;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

internal class AuthState : IState
{
    private GameStateMachine _stateMachine;
    private FirebaseAuth _auth;
    private FormController _formController;
    public AuthState(GameStateMachine StateMachine)
    {
        this._stateMachine = StateMachine;
        _auth = FirebaseAuth.DefaultInstance;
        _formController = MonoBehaviour.FindObjectOfType<FormController>();
    }

    public void Enter()
    {
        FirebaseUser user = _auth.CurrentUser;

        if (user != null)
        {
            // Пользователь авторизован
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            _formController.OpenSignUpForm();
        }
    }

    public void Exit()
    {
        throw new NotImplementedException();
    }
}