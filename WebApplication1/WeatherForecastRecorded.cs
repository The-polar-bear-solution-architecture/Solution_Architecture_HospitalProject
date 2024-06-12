namespace WebApplication1
{
    public class WeatherForecastRecorded
    {
        public static string StreamName = "WeatherForecast";
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}
