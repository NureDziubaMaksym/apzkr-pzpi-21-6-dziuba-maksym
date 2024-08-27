using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Services
{
    public class ApiConnectionService
    {
        private readonly IMqttClient _mqttClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IdGenService _idGenService;

        public ApiConnectionService(IServiceScopeFactory serviceScopeFactory, IdGenService idGenService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _idGenService = idGenService;

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
        }

        public async Task ConnectAndSaveEmotionAsync(int sessionId)
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId("AspNetClient")
                .WithTcpServer("broker.mqttdashboard.com")
                .Build();

            _mqttClient.ConnectedAsync += async e =>
            {
                Console.WriteLine("Connected successfully with MQTT Brokers.");

                // Подписываемся на топик
                await _mqttClient.SubscribeAsync("wokwi-emotion");
                Console.WriteLine("Subscribed to topic.");
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                Console.WriteLine("Disconnected from MQTT Brokers.");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await _mqttClient.ConnectAsync(options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Reconnect failed: {ex.Message}");
                }
            };

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                Console.WriteLine("Received message:");
                var emotion = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Emotion = {emotion}");

                // Сохранение эмоции в базу данных
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                    await SaveEmotionResultAsync(context, sessionId, emotion);
                }
            };

            await _mqttClient.ConnectAsync(options);
        }

        private async Task SaveEmotionResultAsync(DataContext context, int sessionId, string emotion)
        {
            var session = await context.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session == null)
            {
                Console.WriteLine("Session not found.");
                return;
            }

            var result = new Result
            {
                SessionId = session.SessionId,
                AvrEmotion = emotion
            };
            result.ResultId = _idGenService.GenerateNewId<Result>();

            Console.WriteLine(result.AvrEmotion);

            context.Results.Add(result);

            try
            {
                var resultCount = await context.SaveChangesAsync();
                Console.WriteLine($"Number of state entries written to the database: {resultCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }
    }
}
