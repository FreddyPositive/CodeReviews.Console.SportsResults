using HtmlAgilityPack;
using SportsDataReader.Email;
using SportsDataReader.Logging;
using SportsDataReader.Model;

namespace SportsDataReader.Scrapping;

public class Scrapper
{
    private readonly EmailSender _emailSender;
    private static readonly HttpClient _httpClient = new HttpClient();

    public Scrapper(EmailSender emailSender)
    {
        _emailSender = emailSender;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
    }

    public async Task scrapBasketBallDataAsync()
    {
        var logger = new FileLogger("Logs/app-log.txt");

        try
        {
            logger.Info("Starting NBA standings job.");

            var url = "https://www.basketball-reference.com/boxscores/";

            var html = await _httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(html);

            logger.Info("HTML fetched successfully.");

            var easternConferenceTable = htmlDocument.DocumentNode
                   .SelectSingleNode("//table[@id='confs_standings_E']");
            var westernConferenceTable = htmlDocument.DocumentNode
                 .SelectSingleNode("//table[@id='confs_standings_W']");

            var eastTableData = easternConferenceTable.SelectNodes(".//tbody/tr[contains(@class,'full_table')]");
            var westTableData = westernConferenceTable.SelectNodes(".//tbody/tr[contains(@class,'full_table')]");

            logger.Info($"East teams: {eastTableData.Count}");
            logger.Info($"West teams: {westTableData.Count}");

            var eastStandings = new List<TeamStandingDto>();

            foreach (var row in eastTableData)
            {
                var teamNode = row.SelectSingleNode("./th[@data-stat='team_name']");
                var winsNode = row.SelectSingleNode("./td[@data-stat='wins']");
                var lossesNode = row.SelectSingleNode("./td[@data-stat='losses']");
                var winPctNode = row.SelectSingleNode("./td[@data-stat='win_loss_pct']");
                var gbNode = row.SelectSingleNode("./td[@data-stat='gb']");
                var ptsNode = row.SelectSingleNode("./td[@data-stat='pts_per_g']");
                var oppPtsNode = row.SelectSingleNode("./td[@data-stat='opp_pts_per_g']");

                var dto = new TeamStandingDto
                {
                    Team = teamNode.InnerText.Trim(),
                    Wins = int.Parse(winsNode.InnerText),
                    Losses = int.Parse(lossesNode.InnerText),
                    WinPercentage = decimal.Parse(winPctNode.InnerText),
                    GamesBehind = gbNode.InnerText.Trim(),
                    PointsScoredPerGame = decimal.Parse(ptsNode.InnerText),
                    OpponentPointsPerGame = decimal.Parse(oppPtsNode.InnerText)
                };

                eastStandings.Add(dto);
            }
            var westStandings = new List<TeamStandingDto>();

            foreach (var row in westTableData)
            {
                var teamNode = row.SelectSingleNode("./th[@data-stat='team_name']");
                var winsNode = row.SelectSingleNode("./td[@data-stat='wins']");
                var lossesNode = row.SelectSingleNode("./td[@data-stat='losses']");
                var winPctNode = row.SelectSingleNode("./td[@data-stat='win_loss_pct']");
                var gbNode = row.SelectSingleNode("./td[@data-stat='gb']");
                var ptsNode = row.SelectSingleNode("./td[@data-stat='pts_per_g']");
                var oppPtsNode = row.SelectSingleNode("./td[@data-stat='opp_pts_per_g']");

                var dto = new TeamStandingDto
                {
                    Team = teamNode.InnerText.Trim(),
                    Wins = int.Parse(winsNode.InnerText),
                    Losses = int.Parse(lossesNode.InnerText),
                    WinPercentage = decimal.Parse(winPctNode.InnerText),
                    GamesBehind = gbNode.InnerText.Trim(),
                    PointsScoredPerGame = decimal.Parse(ptsNode.InnerText),
                    OpponentPointsPerGame = decimal.Parse(oppPtsNode.InnerText)
                };

                westStandings.Add(dto);
            }

            await _emailSender.SendSportsDataAsync(eastStandings, westStandings);
        }
        catch(Exception ex)
        {
            logger.Error("Unhandled error occurred.", ex);
        }
    }

}
