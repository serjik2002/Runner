using UnityEngine;

[System.Serializable]
public class ScoreData
{

    [SerializeField] public string Email;
    [SerializeField] public string Login;
    [SerializeField] public int Score;

    public ScoreData()
    {

    }

    public ScoreData(string Email, string Login, int Score)
    {
        this.Email = Email;
        this.Login = Login;
        this.Score = Score;
    }

    public override string ToString()
    {
        return $"{Email + " " + Login + " " + Score}";
    }

}
