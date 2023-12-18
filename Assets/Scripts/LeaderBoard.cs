using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
using System;
using Firebase.Extensions;
using TMPro;
using System.Threading.Tasks;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    private AuthUser _authUser;
    private FirebaseAuth _auth;
    private ScoreData _data;
    
    private DatabaseReference _databaseReference;
    [SerializeField] private GameObject _leaderBoardItem;
    
    private List<ScoreData> _usersInDataBase = new List<ScoreData>();

    private async void Start()
    {
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;
        _authUser = FindObjectOfType<AuthUser>();
        await GetUsersFromDataBase();
        FillLeaderBoard();
    }

    public async Task GetUsersFromDataBase()
    {
        await _databaseReference.Child("leaderboard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting leaderboard data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    // Получение данных из снимка (snapshot) и их вывод
                    string key = childSnapshot.Key;
                    string json = childSnapshot.Value.ToString();
                    var scoreData = JsonUtility.FromJson<ScoreData>(json);
                    Debug.Log(scoreData);
                    _usersInDataBase.Add(scoreData);
                    
                }
            }
        });
    }

    public void FillLeaderBoard()
    {
        var text = _leaderBoardItem.GetComponent<TMP_Text>();
        foreach (var item in _usersInDataBase)
        {
            text.text = item.Score.ToString() + " " + item.Login;
            Instantiate(_leaderBoardItem, _parent);
        }
    }

    

    
 
}
