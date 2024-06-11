using System.Text.RegularExpressions;
using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.Dtos;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json.Linq;

namespace IOTBackend.Application.Services
{
    public class LogicProcessorService : ILogicProcessorService
    {
        public static async Task<bool> Process(int sensorValue, string condition)
        {
            try
            {
                // Replace sensor1 with the actual sensor value
                condition = condition.Replace("val", sensorValue.ToString());
                condition = condition
                    .Replace("AND", "&&")
                    .Replace("and", "&&")
                    .Replace("OR", "||")
                    .Replace("or", "||")
                    .Replace("not", "!")
                    .Replace("NOT", "!")
                    .Replace("=", "==");

                // Evaluate the condition using Roslyn scripting
                return await EvaluateConditionAsync(condition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid logic" + ex.Message);
                return false;
            }
        }

        private static async Task<bool> EvaluateConditionAsync(string condition)
        {
            try
            {
                var options = ScriptOptions.Default
                    .WithReferences(AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic && !string.IsNullOrEmpty(asm.Location)))
                    .WithImports("System");

                var result = await CSharpScript.EvaluateAsync<bool>(condition, options);
                return result;
            }
            catch
            {
                return false;
            }
        }
        
        public Dictionary<string, List<string>> GetDevicesWithPins(string json, SensorDataDto sensorData)
        {
            var path = JObject.Parse(json)[sensorData.DeviceId];
            var result = new Dictionary<string, List<string>>();
            ProcessPath(path, result, sensorData);
            return result;
        }
        
        
        static void ProcessPath(JToken token, Dictionary<string, List<string>> result, SensorDataDto sensorData, string currentPin = "")
        {
            foreach (var property in token.Children<JProperty>())
            {
                if (int.TryParse(property.Name, out _))
                {
                    currentPin = property.Name;
                    foreach (var item in property.Value.Children())
                    {
                        ExtractDevices(item, result, sensorData, currentPin);
                    }
                }
            }
        }
        
        static async void ExtractDevices(JToken token, Dictionary<string, List<string>> result, SensorDataDto sensorData,  string currentPin)
        {
            var eligibleForNested = false;
            var devices = token["11"];
            if (devices != null)
            {
                if (!result.ContainsKey(currentPin))
                {
                    result[currentPin] = new List<string>();
                }

                if (devices.Type == JTokenType.Array)
                {
                    foreach (var device in devices)
                    {
                        string condition = $"{token["0"]} {token["1"]} {token["2"]}";
                        var isValid = await Process(sensorData.Value, condition);
                        if (isValid)
                        {
                            eligibleForNested = true;
                            result[currentPin].Add(JsonValueExtractor.ExtractValue(device.ToString()));
                        }
                    }
                }
                else if (devices.Type == JTokenType.Object)
                {
                    foreach (var deviceList in devices.Children())
                    {
                        foreach (var device in deviceList.Children())
                        {
                            string condition = $"{token["0"]} {token["1"]} {token["2"]}";
                            var isValid = await Process(sensorData.Value, condition);
                            if (isValid)
                            {
                                eligibleForNested = true;
                                result[currentPin].Add(JsonValueExtractor.ExtractValue(device.ToString()));
                            }
                                
                        }
                    }
                }
            }
            
            // if (eligibleForNested)
            if (true)
            {
                var nestedItems = token["10"];
                if (nestedItems != null)
                {
                    if (nestedItems.Type == JTokenType.Array)
                    {
                        foreach (var nestedItem in nestedItems)
                        {
                            ExtractDevices(nestedItem, result, sensorData, currentPin);
                        }
                    }
                }
            }
            
        }
        
        private class JsonValueExtractor
        {
            public static string ExtractValue(string json)
            {
                var regex = new Regex("\"([^\"]*)\"");
                var match = regex.Match(json);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }

                return string.Empty;
            }
        }
    }
}
