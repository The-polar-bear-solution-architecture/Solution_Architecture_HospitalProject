using EventStore.Client;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading;

namespace WebApplication1.Controllers
{



    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly EventStoreClient client;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, EventStoreClient client)
        {
            _logger = logger;
            this.client = client;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<List<ResolvedEvent>> Get(CancellationToken cancellationToken)
        {
            var result = client.ReadStreamAsync(
    Direction.Forwards,
    "WeatherForecast",
            StreamPosition.Start,
    cancellationToken: cancellationToken
            );
            return await result.ToListAsync(cancellationToken);

        }


        [HttpPost]
        public async Task<object> PostAsync(WeatherForecast data)
        {
            var weatherForecastRecordedEvent = new WeatherForecastRecorded
            {
                Id = data.Id,
                Date = DateTime.Now,
                Summary = data.Summary,
                TemperatureC = data.TemperatureC
            };

            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(weatherForecastRecordedEvent);

            var eventData = new EventData(Uuid.NewUuid(),
                                           nameof(WeatherForecastRecorded),
                                           utf8Bytes.AsMemory());

            var writeResult = await this.client
                            .AppendToStreamAsync(WeatherForecastRecorded.StreamName,
                                                  StreamState.Any,
                                                  new[] { eventData });

            return writeResult;
        }

        [HttpPut]
        public async Task<object> PutAsync(WeatherForecastUpdate data)
        {
            var weatherForecastUpdatedEvent = new WeatherForecastUpdated
            {
                
                Date = DateTime.Now,
                Summary = data.Summary,
                TemperatureC = data.TemperatureC
            };

            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(weatherForecastUpdatedEvent);

            var eventData = new EventData(Uuid.NewUuid(),
                                           nameof(WeatherForecastUpdated),
                                           utf8Bytes.AsMemory());

            var writeResult = await this.client
                            .AppendToStreamAsync(WeatherForecastUpdated.StreamName,
                                                  StreamState.Any,
                                                  new[] { eventData });

            return writeResult;
        }

        [HttpPatch]
        public async Task<object> PatchAsync(WeatherForecastUpdate data)
        {
            var WeatherForecastTemperatureChangedEvent = new WeatherForecastTemperatureChanged
            {
                Id = data.Id,
                TemperatureC = data.TemperatureC
            };

            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(WeatherForecastTemperatureChangedEvent);

            var eventData = new EventData(Uuid.NewUuid(),
                                           nameof(WeatherForecastUpdated),
                                           utf8Bytes.AsMemory());

            var writeResult = await this.client
                            .AppendToStreamAsync(WeatherForecastUpdated.StreamName,
                                                  StreamState.Any,
                                                  new[] { eventData });

            return writeResult;
        }
    }
}
