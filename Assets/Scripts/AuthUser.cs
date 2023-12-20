using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.Events;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
using Firebase.Database;
using System;
using Firebase;

public class AuthUser : MonoBehaviour
{
    private const string AUTO_LOGIN = "AutoLogin";
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

    [SerializeField] private TMP_Text _errorTextSignUp;
    [SerializeField] private TMP_Text _errorTextLogIn;

    [SerializeField] private Toggle _rememberMe;

    private DatabaseReference _databaseReference;
    private FirebaseAuth _auth;
    private FirebaseUser _user;
    private readonly string _leaderboard = "leaderboard";

    public UnityAction OnSignUpSuccsesfuly;
    public UnityAction OnLogInSuccsesfuly;

    private void Start()
    {
        InitializeFirebase();

        _signUp.onClick.AddListener(SignUp);
        _signIn.onClick.AddListener(LogIn);

        bool key = Convert.ToBoolean(PlayerPrefs.GetInt(AUTO_LOGIN));
        if(key)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    private void InitializeFirebase()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        _user = FirebaseAuth.DefaultInstance.CurrentUser;
    }

    public void SignUp()
    {
        SignUpInputValidate();
        _auth.CreateUserWithEmailAndPasswordAsync(_email.text, _password.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                _errorTextSignUp.text = "SignUp Canceled";
            }
            else if(task.IsFaulted)
            {
            _errorTextSignUp.text = "SignUp Faulted";
                Debug.Log(task.Exception.ToString());
            }
            else if(task.IsCompleted)
            {
                _errorTextSignUp.text = "SignUp Succsesfully";
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
            _errorTextSignUp.text = "Login is empty";
            return;
        }
        else if (_email.text.Length == 0)
        {
            _errorTextSignUp.text = "Email is empty";
            return;
        }
        else if (_password.text.Length < _minPasswordLenght)
        {
            _errorTextSignUp.text = "Password is so short";
            return;
        }
        else if (_password.text != _confirPassword.text)
        {
            _errorTextSignUp.text = "Passwords is not mutch";
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
                _errorTextLogIn.text = "SignIn Canceled";
            }
            else if (task.IsFaulted)
            {

                _errorTextLogIn.text = "SignIn Faulted";
                Debug.Log(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                
                OnLogInSuccsesfuly?.Invoke();
            _errorTextLogIn.text = "SignIn Succsesfully";
                SceneManager.LoadScene("GameScene");
            }
        });
        int isOn = Convert.ToInt32(_rememberMe.isOn);
        PlayerPrefs.SetInt(AUTO_LOGIN, isOn);
    }

    private void LoginInputValidate()
    {
        if (_emailIn.text.Length == 0)
        {
            _errorTextLogIn.text = "Email is empty";
            return;
        }
        else if (_passwordIn.text.Length == 0)
        {
            _errorTextLogIn.text = "Plese, enter password";
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
