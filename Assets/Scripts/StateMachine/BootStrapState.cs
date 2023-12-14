using Firebase.Auth;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BootStrapState : IState
{
    private GameStateMachine _stateMachine;
    private FirebaseAuth _auth;
    public BootStrapState(GameStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
        _auth = FirebaseAuth.DefaultInstance;
    }

    public void Enter()
    {
        _auth.SignOut();
        SceneManager.LoadScene("MainMenu");
        
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
