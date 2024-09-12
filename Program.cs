
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GoonsTracker_bot
{
    class Program
    {
        static async Task Main(string[] args) 
        
        {
            string botToken = "";
            var botClient = new TelegramBotClient(botToken);

            //Message handler
            var cts = new System.Threading.CancellationTokenSource();
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new Telegram.Bot.Polling.ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>()},
                cancellationToken: cts.Token

                );

            //bot info
            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Bot {me.Username} is already hunting for rats!");

            Console.ReadLine();

        }


        static async Task <string> GetGoonsLocationAsync()
        {
           using HttpClient client = new HttpClient();
            var query = new
            {
                query = @"
                {
                    bosses {
                        name
                        locations {
                            name
                        }
                    }
                }"
            };
            try
            {
                //Create the GraphQl request content
                var content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

                //Post request to the API
                var response = await client.PostAsync("https://api.tarkov.dev/graphql", content);
                response.EnsureSuccessStatusCode();

                //Response parsing to JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var bossData = JsonSerializer.Deserialize<GraphQLResponse>(jsonResponse);

                //Find the goons
                foreach(var boss in bossData.Data.Bosses)
                {
                    if (boss.Name.ToLower() == "goons")
                    {
                        return boss.Locations != null ? string.Join(", ", boss.Locations) : "No locations found";
                    }
                }
                return "Goons not found";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return "Error fetching Goons location.";
            }
        }

        //Handle bot updates (pl. messages)

        static async Task HandleUpdateAsync(ITelegramBotClient botclient, Update update, System.Threading.CancellationToken cancellationToken)
        {
            //only process message updates
            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
                return;
               
            //Respond to /goons command
            if (update.Message.Text!.ToLower() == "/goons")
            {
                string goonsLocation = await GetGoonsLocationAsync();
                await botclient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: $"Goons are currently located on: {goonsLocation}",
                    cancellationToken: cancellationToken
                    );
            }
        }

        // Handle errors
        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine($"Error: {exception.Message}");
            return Task.CompletedTask;
        }

    }
    // Classes for deserializing the GraphQL API response
    public class GraphQLResponse
    {
        public BossData Data { get; set; }
    }

    public class BossData
    {
        public Boss[] Bosses { get; set; }
    }

    public class Boss
    {
        public string Name { get; set; }
        public string[] Locations { get; set; }
    }

}