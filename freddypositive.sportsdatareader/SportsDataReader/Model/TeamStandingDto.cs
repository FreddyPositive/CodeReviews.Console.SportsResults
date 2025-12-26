namespace SportsDataReader.Model;

public class TeamStandingDto
{
    public string Team { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public decimal WinPercentage { get; set; }
    public string GamesBehind { get; set; }
    public decimal PointsScoredPerGame { get; set; }
    public decimal OpponentPointsPerGame { get; set; }
}
