using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class UserData : MonoBehaviour
{
    [SerializeField] private TMP_Text _login;
    [SerializeField] private TMP_Text _score;

    private DatabaseReference _databaseReference;
    private ScoreData _scoreData;
    private FirebaseAuth _auth;
    private readonly string _leaderboard = "leaderboard";

    private void Start()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        GetUserData();
        _login.text = _scoreData.Login;
        _score.text = _scoreData.Score.ToString();
    }

    private void GetUserData()
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
                _scoreData = JsonUtility.FromJson<ScoreData>(value);
            }
        });
    }
}
