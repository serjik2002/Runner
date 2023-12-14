using UnityEngine;

[System.Serializable]
public struct ScoreData
{

    public string Email;
    public string Login;
    public int Score;

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
