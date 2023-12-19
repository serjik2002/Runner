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
using System.Linq;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _leaderBoardItem;

    
    private DatabaseReference _databaseReference;
    private List<ScoreData> _usersInDataBase = new List<ScoreData>();

    private void Awake()
    {
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task UpdateLeaderboard()
    {
        await GetUsersFromDataBase();
        FillLeaderBoard();
    }

    public async Task GetUsersFromDataBase()
    {
        _usersInDataBase.Clear();
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
                    _usersInDataBase.Add(scoreData);   
                }
                List<ScoreData> sortedBoard = _usersInDataBase.OrderByDescending(user => user.Score).ToList();
                _usersInDataBase = sortedBoard;
            }
        });
    }

    public void FillLeaderBoard()
    {
        RemoveChildren(_parent);
        var text = _leaderBoardItem.GetComponent<TMP_Text>();
        int index = 1;
        foreach (var item in _usersInDataBase)
        {
            text.text = index.ToString() + " " + item.Score.ToString() + " " + item.Login + " " + item.Score.ToString();
            Instantiate(_leaderBoardItem, _parent);
            index++;
        }
    }

    private void RemoveChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
