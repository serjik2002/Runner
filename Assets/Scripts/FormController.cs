using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormController : MonoBehaviour
{
    [SerializeField] private GameObject _signUpForm;
    [SerializeField] private GameObject _logInForm;


    public void OpenSignUpForm()
    {
        _signUpForm.SetActive(true);
        _logInForm.SetActive(false);
    }

    public void OpenLoginForm()
    {
        _signUpForm.SetActive(false);
        _logInForm.SetActive(true);
    }
}
