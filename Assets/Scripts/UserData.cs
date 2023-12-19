using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using System.Threading.Tasks;

public class UserData : MonoBehaviour
{
    [SerializeField] private TMP_Text _login;
    [SerializeField] private TMP_Text _score;

    private DatabaseReference _databaseReference;
    private ScoreData _scoreData;
    private FirebaseAuth _auth;
    private readonly string _leaderboard = "leaderboard";

    private async void Start()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        await GetUserData();
        _login.text = _scoreData.Login;
        _score.text = "Top Score: " + _scoreData.Score.ToString();
    }

    private async Task GetUserData()
    {
        var userId = _auth.CurrentUser.UserId;

        await _databaseReference.Child(_leaderboard).Child(userId).GetValueAsync().ContinueWith(task =>
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
                Debug.Log(_scoreData);
            }
        });
    }
}
