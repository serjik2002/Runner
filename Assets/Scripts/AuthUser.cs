using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.Events;
using System.Collections;
using TMPro;

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

    private FirebaseAuth _auth;

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
    }

    public void SignUp()
    {
        if (_login.text.Length == 0)
        {
            Debug.Log("Login is empty");
            return;
        }
        if (_email.text.Length == 0)
        {
            Debug.Log("Email is empty");
            return;
        }
        if (_password.text.Length < _minPasswordLenght)
        {
            Debug.Log("Password is so short");
            return;
        }
        if (_password.text != _confirPassword.text)
        {
            Debug.Log("Passwords is not mutch");
            return;
        }
        _auth.CreateUserWithEmailAndPasswordAsync(_email.text, _password.text).ContinueWith(task => {
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
                OnSignUpSuccsesfuly?.Invoke();
                Debug.Log("SignUp Succsesfully");
            }
         
        });
    }

    public void LogIn()
    {
        if (_emailIn.text.Length == 0)
        {
            Debug.Log("Email is empty");
            return;
        }
        if (_passwordIn.text.Length == 0)
        {
            Debug.Log("Plese, enter password");
            return;
        }

        _auth.SignInWithEmailAndPasswordAsync(_emailIn.text, _passwordIn.text).ContinueWith(task =>
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
            }
        });
        
    }

   
}
