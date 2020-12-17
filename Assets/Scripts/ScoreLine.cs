using System;

[Serializable]
public class ScoreLine : IComparable
{
    public string playerName;
    public int score;

    public ScoreLine(string playerName = "", int score = 0)
    {
        this.score = score;
        this.playerName = playerName;
    }

    public int CompareTo(object o)
    {
        ScoreLine line = o as ScoreLine;
        if (line != null)
            return -score.CompareTo(line.score);
        else
            throw new Exception("Невозможно сравнить");
    }
}
