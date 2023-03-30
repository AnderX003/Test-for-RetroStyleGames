using System;

public class GameProgress
{
    public event Action<int> OnScoreChanged;

    private int score;

    public int Score
    {
        get => score;
        private set
        {
            if (score != value)
            {
                OnScoreChanged?.Invoke(value);
            }
            score = value;
        }
    }

    public void Init()
    {
    }

    public void AddScore()
    {
        Score++;
    }
}
