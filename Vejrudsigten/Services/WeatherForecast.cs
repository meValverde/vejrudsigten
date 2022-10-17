using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Vejrudsigten.Services
{
    public static class WeatherForecast
    {
        public static async Task<string> GetForecastAsync(string key)
        {
            WeatherService service = new WeatherService();
            var todayInfo = await service.GetTodaysWeather(key, "Kolding");
            var yesterdayInfo = await service.GetYesterdaysWeather(key, "Kolding");
            return GenerateTitle(yesterdayInfo, todayInfo);
        }

        public static string GenerateTitle(WeatherInfo yesterdayInfo, WeatherInfo todayInfo)
        {
            var temperature = "";
            var condition = "";

            switch (yesterdayInfo.Temperature > todayInfo.Temperature ? 0 : 1)
            {
                case 0:
                    temperature =
                        $"Layer up!, temperature is decreasing from {yesterdayInfo.Temperature} to {todayInfo.Temperature}";
                    break;
                case 1:
                    temperature =
                        $"Is not all bad news, temperatures increasing today by {todayInfo.Temperature - yesterdayInfo.Temperature} degrees";
                    break;
                default:
                    temperature = $"Temperatures remain stable at {todayInfo.Temperature} degrees";
                    break;
            }

            switch (yesterdayInfo.Conditions == todayInfo.Conditions ? 0 : 1)
            {
                case 0 when (todayInfo.Conditions == "Klart vejr"):
                    condition = " - Skies remain clear";
                    break;
                case 0 when (todayInfo.Conditions == "Regn"):
                    condition = " - Wellingtons are also needed today";
                    break;
                case 0 when (todayInfo.Conditions == "Sne"):
                    condition = " - Whoever is praying for snow, please stop";
                    break;
                case 0 when (todayInfo.Conditions == "Skyet"):
                    condition = " - Neither today will you see the sun";
                    break;
                case 1:
                    var yesterdayCondition = TranslateCondition(yesterdayInfo.Conditions);
                    var todayCondition = TranslateCondition(todayInfo.Conditions);
                    condition = $" - {yesterdayCondition} will change to {todayCondition} today";
                    break;
            }

            return temperature + condition;
        }
        
        private static string TranslateCondition(string condition)
        {
            var translatedCondition = "";
            switch (condition)
            {   
                case "Regn":
                    translatedCondition = "Rainy weather";
                    break;
                case "Sne":
                    translatedCondition = "Snowy weather";
                    break;
                case "Skyet":
                    translatedCondition = "Cloudy weather";
                    break;
            }
            return translatedCondition;
        }
    }
}

