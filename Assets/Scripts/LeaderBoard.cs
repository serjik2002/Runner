using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
using System;
using Firebase.Extensions;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    private AuthUser _authUser;
    private FirebaseAuth _auth;
    private ScoreData data;
    private readonly string _leaderboard = "leaderboard";
    private DatabaseReference _databaseReference;
    //test
    [SerializeField] private TMP_Text _textMeshProNickname;
    [SerializeField] private TMP_Text _textMeshProScore;


    private void Start()
    {
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;
        _authUser = FindObjectOfType<AuthUser>();
        _authUser.OnLogInSuccsesfuly += GetUserData;
       
        

    }

    public void CreateUser(string email, string login, int score = 0)
    {
        Debug.Log("User Added in DB");

        var userId = _auth.CurrentUser.UserId;
        ScoreData scoreData = new ScoreData(email, login, score);
        string json = JsonUtility.ToJson(scoreData);

        _databaseReference.Child(_leaderboard).Child(userId).SetValueAsync(json);
    }


    public void GetUserData()
    {
        var userId = _auth.CurrentUser.UserId;
        _databaseReference.Child(_leaderboard).Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
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
                var value = dataSnapshot.Value.ToString();
                var scoreData = JsonUtility.FromJson<ScoreData>(value);
                Debug.Log(scoreData);
                
            }
        });

    }
 
}
