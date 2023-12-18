using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.Events;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
using Firebase.Database;

public class AuthUser : MonoBehaviour
{
    [Header("SignUp")]
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TMP_InputField _confirPassword;
    [SerializeField] private TMP_InputField _login;

    [Header("SignIn")]
    [SerializeField] private TMP_InputField _emailIn;
    [SerializeField] private TMP_InputField _passwordIn;


    [SerializeField] private Button _signIn;
    [SerializeField] private Button _signUp;
    [SerializeField] private int _minPasswordLenght = 6;

    private DatabaseReference _databaseReference;
    private FirebaseAuth _auth;
    private readonly string _leaderboard = "leaderboard";

    public UnityAction OnSignUpSuccsesfuly;
    public UnityAction OnLogInSuccsesfuly;

    private void Start()
    {
        InitializeFirebase();

        _signUp.onClick.AddListener(SignUp);
        _signIn.onClick.AddListener(LogIn);
    }

    private void InitializeFirebase()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SignUp()
    {
        SignUpInputValidate();
        _auth.CreateUserWithEmailAndPasswordAsync(_email.text, _password.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.Log("SignUp Canceled");
            }
            else if(task.IsFaulted)
            {
                Debug.Log("SignUp Faulted");
                Debug.Log(task.Exception.ToString());
            }
            else if(task.IsCompleted)
            {
                Debug.Log("SignUp Succsesfully");
                AddUserDataToDB(_email.text, _login.text);
                OnSignUpSuccsesfuly?.Invoke();

                SceneManager.LoadScene("GameScene");
            }
         
        });
    }
    private void SignUpInputValidate()
    {
        if (_login.text.Length == 0)
        {
            Debug.Log("Login is empty");
            return;
        }
        else if (_email.text.Length == 0)
        {
            Debug.Log("Email is empty");
            return;
        }
        else if (_password.text.Length < _minPasswordLenght)
        {
            Debug.Log("Password is so short");
            return;
        }
        else if (_password.text != _confirPassword.text)
        {
            Debug.Log("Passwords is not mutch");
            return;
        }
    }

    public void LogIn()
    {
        LoginInputValidate();
        _auth.SignInWithEmailAndPasswordAsync(_emailIn.text, _passwordIn.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignIn Canceled");
            }
            else if (task.IsFaulted)
            {

                Debug.Log("SignIn Faulted");
                Debug.Log(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                
                OnLogInSuccsesfuly?.Invoke();
                Debug.Log("SignIn Succsesfully");
                SceneManager.LoadScene("GameScene");
            }
        });
        
    }

    private void LoginInputValidate()
    {
        if (_emailIn.text.Length == 0)
        {
            Debug.Log("Email is empty");
            return;
        }
        else if (_passwordIn.text.Length == 0)
        {
            Debug.Log("Plese, enter password");
            return;
        }
    }

    public void AddUserDataToDB(string email, string login, int score = 0)
    {
        var userId = _auth.CurrentUser.UserId;
        ScoreData scoreData = new ScoreData(email, login, score);
        string json = JsonUtility.ToJson(scoreData);

        _databaseReference.Child(_leaderboard).Child(userId).SetValueAsync(json);
    }
    
}
