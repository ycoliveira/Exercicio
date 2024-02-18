using Newtonsoft.Json;
using static Program;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        string urlTeam1 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}";
        string urlTeam2 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}";

        int totalGoals = 0;


        using (HttpClient client = new HttpClient())
        {
            totalGoals += await GetTotalGoalsForTeam(client, urlTeam1, team);
            totalGoals += await GetTotalGoalsForTeam(client, urlTeam2, team);
        }

        return totalGoals;
    }

    public static async Task<int> GetTotalGoalsForTeam(HttpClient httpClient, string url, string team)
    {
        int totalGoals = 0;
        int currentPage = 1;
        int totalPages = 0;

        do
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{url}&page={currentPage}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            FootballMatchListResponse footballMatchListResponse = JsonConvert.DeserializeObject<FootballMatchListResponse>(responseBody);

            if (totalPages == 0)
            {
                totalPages = footballMatchListResponse.total_pages;
            }

            foreach (var match in footballMatchListResponse.data)
            {
                if (match.team1 == team)
                {
                    totalGoals += Convert.ToInt32(match.team1goals);
                }
                else if (match.team2 == team)
                {
                    totalGoals += Convert.ToInt32(match.team2goals);
                }
            }

            currentPage++;
        }

        while (currentPage <= totalPages);


        return totalGoals;
    }

    public class FootballMatchResponse
    {
        public string competition { get; set; }
        public int year { get; set; }
        public string round { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public string team1goals { get; set; }
        public string team2goals { get; set; }
    }

    public class FootballMatchListResponse
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public FootballMatchResponse[] data { get; set; }
    }

}