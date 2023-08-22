using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDBTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDynamoDBContext _dynamoDBContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDynamoDBContext dynamoDBContext)
        {
            _logger = logger;
            _dynamoDBContext = dynamoDBContext;
        }

        #region Get Selected Date Weather Record
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get(string City = "Peshawar")
        {
             return await _dynamoDBContext.QueryAsync<WeatherForecast>(City,QueryOperator.BeginsWith, new object[] { "2023-08-17"})
                .GetRemainingAsync();

            //return GenerateDummyWeatherForeCast(City);
        }
        #endregion

        #region Read All Data from Table

        /*public async Task<IEnumerable<WeatherForecast>> Get()
          {
              return await _dynamoDBContext.QueryAsync<WeatherForecast>("")
                 .GetRemainingAsync();

              //return GenerateDummyWeatherForeCast(City);
          }*/
        #endregion

        #region Save Weather Forecast in DynamoDB Table
        [HttpPost(Name = "GetWeatherForecast")]
        public async Task Post(string city)
        {
            var data = GenerateDummyWeatherForeCast(city);
            foreach (var item in data)
            {
                await _dynamoDBContext.SaveAsync(item);
            }
        }
        #endregion

        #region Update Single Weather Forecast in DynamoDB Table
        [HttpPut(Name = "GetWeatherForecast")]
        public async Task Put(string city)
        {   
            var specified=   await _dynamoDBContext.LoadAsync<WeatherForecast>(city,"2023-08-17");
            specified.Summary = "Test";
            await _dynamoDBContext.SaveAsync(specified);
        }
        #endregion

        #region Delete Single Weather Forecast in DynamoDB Table
        [HttpDelete(Name = "GetWeatherForecast")]
        public async Task Delete(string city)
        {
            var specified = await _dynamoDBContext.LoadAsync<WeatherForecast>(city, "2023-08-18");
            await _dynamoDBContext.DeleteAsync(specified);
        }
        #endregion

        #region Generate Weather ForeCast Dummy Data
        private static IEnumerable<WeatherForecast> GenerateDummyWeatherForeCast(string City)
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                City = City,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)).ToLongDateString(),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        #endregion
    }
}