using SportsDataReader.Logging;
using SportsDataReader.Model;
using System.Text;

namespace SportsDataReader.Email;

class EmailBodyFormatter
{
    public string BuildStandingsHtml(List<TeamStandingDto> eastStandings, List<TeamStandingDto> westStandings)
    {
        var sb = new StringBuilder();
        var logger = new FileLogger("Logs/app-log.txt");
        sb.Append($"<p>Hello Team,<br>\r\n\r\nBelow are the latest NBA conference standings fetched from Basketball Reference.</p>");
        sb.Append($"<h3><b>Conference Standings</b></h3>");
        sb.Append("<table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse;'>");

        // Header
        sb.Append("<thead><tr>");
        sb.Append("<th>Eastern Conference</th>");
        sb.Append("<th>W</th>");
        sb.Append("<th>L</th>");
        sb.Append("<th>W/L %</th>");
        sb.Append("<th>GB</th>");
        sb.Append("<th>PS/G</th>");
        sb.Append("<th>PA/G</th>");
        sb.Append("</tr></thead>");

        // Body
        sb.Append("<tbody>");
        foreach (var team in eastStandings)
        {
            sb.Append("<tr>");
            sb.Append($"<td>{team.Team}</td>");
            sb.Append($"<td>{team.Wins}</td>");
            sb.Append($"<td>{team.Losses}</td>");
            sb.Append($"<td>{team.WinPercentage}</td>");
            sb.Append($"<td>{team.GamesBehind}</td>");
            sb.Append($"<td>{team.PointsScoredPerGame}</td>");
            sb.Append($"<td>{team.OpponentPointsPerGame}</td>");
            sb.Append("</tr>");
        }
        sb.Append("</tbody>");

        sb.Append("</table><br/>");

        sb.Append("<table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse;'>");

        // Header
        sb.Append("<thead><tr>");
        sb.Append("<th>Western Conference</th>");
        sb.Append("<th>W</th>");
        sb.Append("<th>L</th>");
        sb.Append("<th>W/L %</th>");
        sb.Append("<th>GB</th>");
        sb.Append("<th>PS/G</th>");
        sb.Append("<th>PA/G</th>");
        sb.Append("</tr></thead>");

        // Body
        sb.Append("<tbody>");
        foreach (var team in westStandings)
        {
            sb.Append("<tr>");
            sb.Append($"<td>{team.Team}</td>");
            sb.Append($"<td>{team.Wins}</td>");
            sb.Append($"<td>{team.Losses}</td>");
            sb.Append($"<td>{team.WinPercentage}</td>");
            sb.Append($"<td>{team.GamesBehind}</td>");
            sb.Append($"<td>{team.PointsScoredPerGame}</td>");
            sb.Append($"<td>{team.OpponentPointsPerGame}</td>");
            sb.Append("</tr>");
        }
        sb.Append("</tbody>");

        sb.Append("</table><br/>");

        sb.Append($"Source : https://www.basketball-reference.com/boxscores/<br>");
        sb.Append("Regards,<br>");
        sb.Append("Sports Data Providers");
        logger.Info("Email Body generated.");
        return sb.ToString();

    }
}
