using Microsoft.Extensions.Configuration;
using SportsDataReader.Email;
using SportsDataReader.Scrapping;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

EmailSender emailSender = new(config);
Scrapper scrapper = new(emailSender);

await scrapper.scrapBasketBallDataAsync();

Console.WriteLine("Finished.");