using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine.UI;
using System;
using Firebase.Extensions;

public class LeaderBoard : MonoBehaviour
{
    private AuthUser _authUser;
    private FirebaseDatabase _database;
    private FirebaseAuth _auth;
    ScoreData data;
    private readonly string _leaderboard = "leaderboard";

    //test
    [SerializeField] private TMPro.TMP_Text _textMeshProNickname;
    [SerializeField] private TMPro.TMP_Text _textMeshProScore;


    private void Start()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _authUser = FindObjectOfType<AuthUser>();
        _authUser.OnLogInSuccsesfuly.AddListener(GetUserData);

    }

    public void CreateUser(string email, string login, int score = 0)
    {
        Debug.Log("User Added in DB");

        var userId = _auth.CurrentUser.UserId;
        ScoreData scoreData = new ScoreData(email, login, score);
        string json = JsonConvert.SerializeObject(scoreData);

        FirebaseDatabase.DefaultInstance.RootReference.Child(_leaderboard).Child(userId).SetValueAsync(json);
    }


    public void GetUserData()
    {
        Debug.Log("GetUserData");
        var userId = _auth.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.RootReference.Child(_leaderboard).Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.Log(task.Exception.ToString());
            }
            else if (task.Result == null)
            {
                Debug.Log("error");
            }
            else
            {
                DataSnapshot dataSnapshot = task.Result;
                Debug.Log(dataSnapshot.Child("Email").Value.ToString());
            }
        });

        
       
        
    }

    //test
    public void UpdateData()
    {
        _textMeshProNickname.text = data.Login;
        _textMeshProScore.text = data.Score.ToString();
    }
}
